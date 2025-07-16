using System.Text;

namespace Gomoku;

public class GameEngine2003 : IGameEngine
{
	public void SetMoves(List<(int x, int y, int player)> moves)
	{
		// Teljes nullázás
		this.moves = 0;
		player = 0;
		lastMove = -1;

		// Belső tábla ürítése (csak nem-border mezők)
		for (int p = 0; p < boardk; p++)
		{
			if (board[p].z != 3)
			{
				board[p].z = 0;
			}
		}

		// Prioritások nullázása
		for (int k = 0; k < 2; k++)
		{
			sum[k] = 0;
			for (int level = 1; level <= 3; level++)
			{
				goodMoves[level, k] = 0;
			}
		}

		// Moves visszajátszása egyesével
		foreach (var (x, y, playerNum) in moves)
		{
			int p = (y + 6) * height2 + x + 1;

			// Színkonverzió javítás
			if (playerNum == 1)      // GameHistory: 1=Black  
				board[p].z = 2;      // GameEngine: 2=Black
			else if (playerNum == 2) // GameHistory: 2=White
				board[p].z = 1;      // GameEngine: 1=White

			this.moves++;
			evaluate(p);
			lastMove = p;
		}

		// Player beállítása a lépések számából
		player = this.moves % 2;
	}

	public string GetDebugState()
	{
		var sb = new StringBuilder();

		sb.AppendLine("=== GameEngine2003 Debug State ===");
		sb.AppendLine($"moves: {moves}");
		sb.AppendLine($"player: {player} (0=white, 1=black)");
		sb.AppendLine($"width: {width}, height: {height}");
		sb.AppendLine($"lastMove: {lastMove}");
		sb.AppendLine($"dpth: {dpth}, depth: {depth}");

		sb.AppendLine("\n--- Internal Board State ---");
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				int p = (x + 6) * height2 + y + 1;
				char c = board[p].z switch
				{
					0 => '.',
					1 => 'W', // White
					2 => 'B', // Black  
					3 => '#', // Border
					_ => '?'
				};
				sb.Append(c);
			}
			sb.AppendLine();
		}

		sb.AppendLine($"\n--- Priority Lists ---");
		for (int level = 1; level <= 3; level++)
		{
			for (int pl = 0; pl <= 1; pl++)
			{
				int count = 0;
				int p = goodMoves[level, pl];
				while (p != 0 && count < 10) // Max 10 to avoid infinite loops
				{
					count++;
					p = board[p].h[pl].nxt;
				}
				sb.AppendLine($"goodMoves[{level},{pl}]: {count} items");
			}
		}

		sb.AppendLine($"\n--- Sum priorities ---");
		sb.AppendLine($"sum[0] (white): {sum[0]}");
		sb.AppendLine($"sum[1] (black): {sum[1]}");

		return sb.ToString();
	}

	class Tprior
	{
		public int pv, ps0, ps1, ps2, ps3; //ohodnocení ve 4 smìrech a jejich souèet
		public int i;       //v kterém je seznamu dobrých tahù
		public int nxt, pre; //další a pøedchozí prvky seznamu dobrých tahù
	};

	struct Tsquare
	{
		public int z;       //0=prázndné, 1=bílý, 2=èerný, 3=hranice okna
		public Tprior[] h;  //ohodnocení pro oba hráèe
		public int x, y;        //souøadnice
	};

	int
		player, //hráè na tahu, 0=bílý, 1=èerný
		moves,  //poèet tahù
		width,  //šíøka hracího pole
		height, //výška hracího pole
		height2;//height+2
	int[] diroff = new int[9]; //vzdálenosti na sousední políèka v hracím poli

	//---------------------------------------------------------------------------
	const int  //konstanty pro ohodnocovací funkci
		H10 = 2, H11 = 6, H12 = 10,
		H20 = 23, H21 = 158, H22 = 175,
		H30 = 256, H31 = 511, H4 = 2047;
	int[,] priority = { { 0, 1, 1 }, { H10, H11, H12 }, { H20, H21, H22 }, { H30, H31, 0 }, { H4, 0, 0 } };

	int[] sum = { 0, 0 };   //celkové souèty priorit všech polí pro oba hráèe
	int dpth = 0, depth; //hloubka rekurze
	int[] D = { 7, 5, 4 };

	const int McurMoves = 384;
	const int MwinMoves = 400;
	int[]
		curMoves = new int[McurMoves],  //buffer na zpracovávané tahy
		winMoves1 = new int[MwinMoves], //buffer na výherní kombinace
		winEval = new int[MwinMoves];

	int[,]
		goodMoves = new int[4, 2],  //seznamy políèek s velkým ohodnocením; pro oba hráèe
		winMove = new int[2, 2];        //místo, kde už se dá vyhrát; pro oba hráèe

	int
		UwinMoves,
		lastMove,
		bestMove;  //výsledný tah poèítaèe

	Tsquare[] board;  //hrací plocha
	int boardk; //konec hrací plochy
	Random generator;
	//---------------------------------------------------------------------------
	int max(int a, int b)
	{
		return (a > b) ? a : b;
	}
	int abs(int x)
	{
		return x >= 0 ? x : -x;
	}
	int distance(int p1, int p2)
	{
		return max(abs(board[p1].x - board[p2].x),
			abs(board[p1].y - board[p2].y));
	}
	//---------------------------------------------------------------------------
	public StoneCoordinate GetBestMove(StoneColor[,] board, StoneColor colorOnMove, TimeSpan thinkingTime)
	{
		int p, x, y;

		//nastav hloubku pøemýšlení podle zbývajícího èasu
		depth = 4 + (int)thinkingTime.TotalSeconds / 60;
		//pøi prvním tahu zjisti, jestli jsem bílý nebo èerný
		if (moves == 0)
		{
			player = 0;
			if (colorOnMove == StoneColor.White) player = 1;
		}
		//najdi v hracím poli poslední tah soupeøe
		p = 6 * height2 + 1;
		for (x = 0; x < width; x++)
		{
			for (y = 0; y < height; y++)
			{
				switch (board[y, x])
				{
					case StoneColor.White:
						if (this.board[p].z != 1)
						{
							doMove(p);
							goto mujTah;
						}
						break;
					case StoneColor.Black:
						if (this.board[p].z != 2)
						{
							doMove(p);
							goto mujTah;
						}
						break;
				}
				p++;
			}
			p += 2;
		}
		//tah soupeøe nenalezen => já zaèínám		
		player = 0;
		if (colorOnMove == StoneColor.Black) player = 1;
		//proveï svùj tah
		mujTah:
		computer1();
		return new StoneCoordinate(this.board[lastMove].y, this.board[lastMove].x, _boardModel.BoardSize);
	}
	//---------------------------------------------------------------------------
	private readonly BoardModel _boardModel;

	public GameEngine2003(BoardModel boardModel)
	{
		_boardModel = boardModel;
		int x, y, k;
		int p;
		Tprior pr;

		generator = new Random();
		//alokuj hrací plochu
		width = height = _boardModel.BoardSize;
		height2 = height + 2;
		board = new Tsquare[(width + 12) * (height2)]; //jednorozmìrné pole !
		boardk = (width + 6) * height2;
		//offsety pro pohyb do všech osmi smìrù
		diroff[0] = 1;
		diroff[4] = -diroff[0];
		diroff[1] = (1 + height2);
		diroff[5] = -diroff[1];
		diroff[2] = height2;
		diroff[6] = -diroff[2];
		diroff[3] = (-1 + height2);
		diroff[7] = -diroff[3];
		diroff[8] = 0;

		//vynuluj pole
		p = 0;
		for (x = -5; x <= width + 6; x++)
		{
			for (y = 0; y <= height + 1; y++)
			{
				board[p].z = (x < 1 || y < 1 || x > width || y > height) ? 3 : 0;
				board[p].x = x - 1;
				board[p].y = y - 1;
				board[p].h = new Tprior[2];
				for (k = 0; k < 2; k++)
				{
					board[p].h[k] = pr = new Tprior();
					pr.i = 0;
					pr.pv = 4;
					pr.ps0 = pr.ps1 = pr.ps2 = pr.ps3 = 1;
				}
				p++;
			}
		}
		moves = 0;
		//vytvoø pomocnou tabulku pro zrychlení ohodnocovací funkce
		gen();
	}
	//---------------------------------------------------------------------------
	//udìlá tah na pole p
	bool doMove(int p)
	{
		if (board[p].z != 0) return false;
		board[p].z = player + 1;
		player = 1 - player;
		//zvyš poèítadlo tahù
		moves++;
		//pøepoèítej ohodnocení
		evaluate(p);
		lastMove = p;
		return true;
	}
	//---------------------------------------------------------------------------
	short[,] K = new short[2, 262144]; //ohodnocení pro všechny kombinace 9 polí
	static int[] comb = new int[10];
	static int ind;
	static int[] n = new int[4];
	//---------------------------------------------------------------------------
	void gen2(int pos)
	{
		int pb, pe, a1, a2;
		int n1, n2, n3;
		int s;

		if (pos == 9)
		{
			a1 = a2 = 0;
			if (comb[4] == 0)
			{
				n1 = n[1]; n2 = n[2]; n3 = n[3];
				pb = 0;
				pe = 4;
				while (pe != 9)
				{
					if (n3 == 0)
					{
						if (n2 == 0)
						{
							s = 0;
							if (comb[pb] == 0 && comb[pe + 1] < 2 && pb != 4)
							{
								s++;
								if (comb[pe] == 0 && pe != 4) s++;
							}
							int pri = priority[n1, s];
							if (a1 < pri) a1 = pri;
						}
						if (n1 == 0)
						{
							s = 0;
							if (comb[pb] == 0 && (comb[pe + 1] & 1) == 0 && pb != 4)
							{
								s++;
								if (comb[pe] == 0 && pe != 4) s++;
							}
							int pri = priority[n2, s];
							if (a2 < pri) a2 = pri;
						}
					}
					switch (comb[++pe])
					{
						case 1: n1++; break;
						case 2: n2++; break;
						case 3: n3++; break;
					}
					switch (comb[pb++])
					{
						case 1: n1--; break;
						case 2: n2--; break;
						case 3: n3--; break;
					}
				}
			}
			K[0, ind] = (short)a1;
			K[1, ind] = (short)a2;
			ind++;
		}
		else
		{
			//vygeneruj postupnì všechny kombinace 
			for (int z = 0; z < 4; z++)
			{
				comb[pos] = z;
				gen2(pos + 1);
			}
		}
	}

	void gen1(int pos)
	{
		if (pos == 5) gen2(pos);
		else
		{
			for (int z = 0; z < 4; z++)
			{
				comb[pos] = z;
				n[z]++;
				gen1(pos + 1);
				n[z]--;
			}
		}
	}

	void gen()
	{
		ind = 0;
		gen1(0);
	}
	//---------------------------------------------------------------------------
	//pøepoèítá ohodnocení polí do vzdálenosti 4 od políèka p0
	void evaluate(int p0)
	{
		int i, k, m, s, h;
		Tprior pr;
		int p, q, qk, pe, pk1;
		int ind;
		int pattern;

		//zaplnìné pole odstraò ze seznamu a dej mu nulovou prioritu
		if (board[p0].z != 0)
		{
			for (k = 0; k < 2; k++)
			{
				pr = board[p0].h[k];
				if (pr.pv != 0)
				{
					if (pr.i != 0)
					{
						board[pr.nxt].h[k].pre = pr.pre;
						if (pr.pre != 0) board[pr.pre].h[k].nxt = pr.nxt;
						else goodMoves[pr.i, k] = pr.nxt;
						pr.i = 0;
					}
					sum[k] -= pr.pv;
					pr.pv = pr.ps0 = pr.ps1 = pr.ps2 = pr.ps3 = 0;
				}
			}
		}
		//zpracuj všechny 4 smìry
		for (i = 0; i < 4; i++)
		{
			s = diroff[i];
			pk1 = p0;
			pk1 += s * 5;
			pe = p0;
			p = p0;
			for (m = 4; m > 0; m--)
			{
				p -= s;
				if (board[p].z == 3)
				{
					pe += s * m;
					p += s;
					break;
				}
			}
			pattern = 0;
			qk = pe;
			qk -= s * 9;
			for (q = pe; q != qk; q -= s)
			{
				pattern *= 4;
				pattern += board[q].z;
			}
			while (board[p].z != 3)
			{
				if (board[p].z == 0)
				{
					for (k = 0; k < 2; k++)
					{ //pro oba hráèe
						pr = board[p].h[k];
						//oprav prioritu v jednom smìru
						h = K[k, pattern];
						switch (i)
						{
							case 0:
								m = pr.ps0; pr.ps0 = h;
								break;
							case 1:
								m = pr.ps1; pr.ps1 = h;
								break;
							case 2:
								m = pr.ps2; pr.ps2 = h;
								break;
							case 3:
								m = pr.ps3; pr.ps3 = h;
								break;
						}
						m = h - m;
						if (m != 0)
						{
							sum[k] += m;
							pr.pv += m;
							//podle ohodnocení urèi seznam
							ind = 0;
							if (pr.pv >= H21)
							{
								ind++;
								if (pr.pv >= 2 * H21)
								{
									ind++;
									if (pr.pv >= H4) ind++;
								}
							}
							//pøehoï políèko do jiného seznamu
							if (ind != pr.i)
							{
								//odpoj
								if (pr.i != 0)
								{
									board[pr.nxt].h[k].pre = pr.pre;
									if (pr.pre != 0) board[pr.pre].h[k].nxt = pr.nxt;
									else goodMoves[pr.i, k] = pr.nxt;
								}
								//pøipoj
								if ((pr.i = ind) != 0)
								{
									q = pr.nxt = goodMoves[ind, k];
									goodMoves[ind, k] = board[q].h[k].pre = p;
									pr.pre = 0;
								}
							}
						}
					}
				}
				p += s;
				if (p == pk1) break;
				//rotuj pattern vpravo; zleva naèti další políèko
				pe += s;
				pattern >>= 2;
				pattern += board[pe].z << 16;
			}
		}
	}
	//---------------------------------------------------------------------------
	//hlavní rekurzivní funkce
	//zjistí, jestli prohraju nebo vyhraju
	//pøi dpth==0 nastaví promìnnou tah
	int alfabeta(int player1, int UcurMoves, int logWin, int last, int strike)
	{
		int p, q, t, defendMoves1, defendMoves2, UwinMoves0;
		int y, m;
		int i, j, s;
		int pr, hr;
		int mustDefend, mustAttack;

		//když už jsou ètyøi v øadì, tak táhni bez rozmýšlení
		p = goodMoves[3, player1];
		if (p != 0)
		{
			if (logWin != 0 && (strike & 1) != 0) winMoves1[UwinMoves++] = p;
			return 1000 - dpth; //vyhrál jsem :)
		}
		int player2 = 1 - player1;
		p = goodMoves[3, player2];
		if (p != 0)
		{
			board[p].z = player1 + 1;
			evaluate(p);
			if ((strike & 1) != 0)
				y = -alfabeta(player2, UcurMoves, logWin, last, 2);
			else
				y = -alfabeta(player2, UcurMoves, logWin, last, 1);
			board[p].z = 0;
			evaluate(p);
			if (logWin != 0 && y != 0 && ((y > 0) == ((strike & 1) != 0))) winMoves1[UwinMoves++] = p;
			return y;
		}

		//nejdøíve najdi všechny dobré tahy a pøekopíruj je do statického pole
		int Utahy0 = UcurMoves;
		if ((strike & 1) == 0) hr = player2; else hr = player1;
		mustDefend = mustAttack = 0;
		p = goodMoves[2, player1];
		if (p != 0)
		{
			mustAttack++;
			do
			{
				//už mám tøi v øadì => mìl bych vyhrát
				if (logWin == 0 && board[p].h[player1].pv >= H31)
				{
					if (dpth == 0) bestMove = p;
					return 999 - dpth;
				}
				if (UcurMoves == McurMoves) break;
				pr = board[p].h[hr].pv;
				for (q = UcurMoves++; q > Utahy0 &&
					board[curMoves[q - 1]].h[hr].pv < pr; q--)
				{
					curMoves[q] = curMoves[q - 1];
				}
				curMoves[q] = p;
				p = board[p].h[player1].nxt;
			} while (p != 0);
		}
		defendMoves1 = UcurMoves;
		for (p = goodMoves[2, player2]; p != 0; p = board[p].h[player2].nxt)
		{
			//soupeø má tøi v øadì => musím se bránit
			if (board[p].h[player2].pv >= H30 + H21)
			{
				if (mustDefend == 0) mustDefend = 1;
				if (board[p].h[player2].pv >= H31) mustDefend = 2;
			}
			else
			{
				if (mustAttack != 0) continue;
			}
			if (UcurMoves == McurMoves) break;
			pr = board[p].h[hr].pv;
			for (q = UcurMoves++; q > defendMoves1 &&
				board[curMoves[q - 1]].h[hr].pv < pr; q--)
			{
				curMoves[q] = curMoves[q - 1];
			}
			curMoves[q] = p;
		}
		defendMoves2 = UcurMoves;

		if (dpth < depth)
		{
			//dívej se jen na okolí posledního tahu
			if (strike < 2 && last != 0)
			{
				for (i = 0; i < 8; i++)
				{
					s = diroff[i];
					p = last;
					p += s;
					for (j = 0; j < 4 && (board[p].z != 3); j++, p += s)
					{
						if ((strike & 1) == 0 && board[p].h[player2].i == 1
							&& (mustAttack == 0 || board[p].h[player2].pv >= H30)
							|| board[p].h[player1].i == 1 &&
							(mustDefend == 0 || board[p].h[player1].pv >= H30))
						{
							if (UcurMoves < McurMoves)
							{
								pr = board[p].h[hr].pv;
								for (q = UcurMoves++; q > defendMoves2 &&
									board[curMoves[q - 1]].h[hr].pv < pr; q--)
								{
									curMoves[q] = curMoves[q - 1];
								}
								curMoves[q] = p;
							}
						}
					}
				}
			}
			else
			{
				//obrana
				if (strike == 2 && mustDefend < 2)
				{
					for (p = goodMoves[1, player2]; p != 0; p = board[p].h[player2].nxt)
					{
						if (UcurMoves == McurMoves) break;
						if ((last == 0 || distance(p, last) < D[mustDefend])
							&& (mustAttack == 0 || board[p].h[player2].pv >= H30)
							)
						{
							if (UcurMoves == McurMoves) break;
							pr = board[p].h[hr].pv;
							for (q = UcurMoves++; q > defendMoves2 &&
								board[curMoves[q - 1]].h[hr].pv < pr; q--)
							{
								curMoves[q] = curMoves[q - 1];
							}
							curMoves[q] = p;
						}
					}
					defendMoves2 = UcurMoves;
				}
				//útok
				for (p = goodMoves[1, player1]; p != 0; p = board[p].h[player1].nxt)
				{
					if (UcurMoves == McurMoves) break;
					if ((last == 0 || distance(p, last) < 7)
						&& (mustDefend == 0 || board[p].h[player1].pv >= H30)
						)
					{
						if (UcurMoves == McurMoves) break;
						pr = board[p].h[hr].pv;
						for (q = UcurMoves++; q > defendMoves2 &&
							board[curMoves[q - 1]].h[hr].pv < pr; q--)
						{
							curMoves[q] = curMoves[q - 1];
						}
						curMoves[q] = p;
					}
				}
			}
		}

		if (Utahy0 == UcurMoves)
			return 0; //nelze nikde zaútoèit nebo už jsem moc hluboko

		//dobré tahy jsou v poli curMoves => ohodno je a vyber nejlepší
		UwinMoves0 = UwinMoves;
		m = -0x7ffe;
		for (t = Utahy0; t < UcurMoves; t++)
		{
			dpth++;
			p = curMoves[t];
			//proveï tah
			board[p].z = player1 + 1;
			evaluate(p);
			//rekurze
			if ((strike & 1) != 0)
			{
				if (t >= defendMoves2 || t < defendMoves1)
					//útoèný tah, aktualizuj místo posledního útoku
					y = -alfabeta(player2, UcurMoves, logWin, p, 0);
				else
					//obranný tah, nemìním políèko posledního útoku
					//soupeø získal tah navíc a mùže se bránit, kde bude chtít
					y = -alfabeta(player2, UcurMoves, logWin, last, 2);
			}
			else
			{
				y = -alfabeta(player2, UcurMoves, logWin, last, 1);
			}
			//smaž, co jsi pøidal
			board[p].z = 0;
			evaluate(p);
			dpth--;
			if (y > 0)
			{
				//vyhraju
				if (dpth == 0) bestMove = p;
				if (logWin != 0 && (strike & 1) != 0) winMoves1[UwinMoves++] = p;
				return y;
			}
			if (y == 0)
			{
				if ((strike & 1) == 0)
				{
					//ubráním se
					UwinMoves = UwinMoves0;
					if (dpth == 0) bestMove = p;
					return y;
				}
				m = y;
			}
			else if (y >= m)
			{
				//asi prohraju, musím zkoušet další možné tahy
				if (logWin != 0 && (strike & 1) == 0) { winMoves1[UwinMoves++] = p; logWin = 0; }
				if (dpth == 0)
					//vyber tah, kterým prohraju co nejpozdìji
					if (y > m || board[p].h[player2].pv > board[bestMove].h[player2].pv)
						bestMove = p;
				m = y;
			}
		}
		return m;
	}
	//---------------------------------------------------------------------------
	int try4(int player1, int last)
	{
		int i, j, s;
		int p, p2 = 0, y = 0;

		p = goodMoves[3, player1];
		if (p != 0)
		{
			winMoves1[UwinMoves++] = p;
			return p; //vyhrál jsem
		}
		int player2 = 1 - player1;

		for (i = 0; i < 8; i++)
		{
			s = diroff[i];
			p = last;
			p += s;
			for (j = 0; j < 4 && (board[p].z != 3); j++, p += s)
			{
				if (board[p].h[player1].pv >= H30)
				{
					//útok
					board[p].z = player1 + 1;
					evaluate(p);
					if (goodMoves[3, player2] == 0)
					{
						p2 = goodMoves[3, player1];
						if (p2 != 0)
						{
							//obrana - jen jediná možnost
							board[p2].z = 2 - player1;
							evaluate(p2);
							//rekurze
							y = try4(player1, p);
							board[p2].z = 0;
							evaluate(p2);
						}
					}
					board[p].z = 0;
					evaluate(p);
					if (y != 0)
					{
						winMoves1[UwinMoves++] = p2;
						winMoves1[UwinMoves++] = p;
						return p;
					}
				}
			}
		}
		return 0;
	}
	//---------------------------------------------------------------------------
	//zkoušej jen vynucené tahy, kdy útoèník dìlá jen ètveøice
	//hloubka rekurze není omezena 
	int try4(int player1)
	{
		int p, p2, y = 0, t;
		int j;

		UwinMoves = 0;
		t = 0;
		for (j = 1; j <= 2; j++)
		{
			for (p = goodMoves[j, player1]; p != 0; p = board[p].h[player1].nxt)
			{
				if (board[p].h[player1].pv >= H30)
				{
					if (t == McurMoves) break;
					curMoves[t++] = p;
				}
			}
		}
		for (t--; t >= 0; t--)
		{
			p = curMoves[t];
			board[p].z = player1 + 1;
			evaluate(p);
			if (goodMoves[3, 1 - player1] == 0)
			{
				p2 = goodMoves[3, player1];
				if (p2 != 0)
				{
					board[p2].z = 2 - player1;
					evaluate(p2);
					y = try4(player1, p);
					board[p2].z = 0;
					evaluate(p2);
				}
				board[p].z = 0;
				evaluate(p);
				if (y != 0)
				{
					winMoves1[UwinMoves++] = p2;
					winMoves1[UwinMoves++] = p;
					return p;
				}
			}
		}
		return 0;
	}
	//---------------------------------------------------------------------------
	int alfabeta(int strike, int player1, int logWin, int last)
	{
		/* int y=0;
		 if(depth>5 && player==1){
			 depth-=5;
			 y=alfabeta(player1,curMoves,logWin,last,strike);
			 setDepth();
		 }
		 if(!y) y=*/
		return alfabeta(player1, 0, logWin, last, strike);
	}
	//---------------------------------------------------------------------------
	//zjisti ohodnocení políèka p0 pro hráèe player1
	int getEval(int player1, int p0)
	{
		int i, s, y, c1, c2, n;
		int p;

		y = 0;
		//podívej se na okolní políèka
		c1 = c2 = 0;
		for (i = 0; i < 8; i++)
		{
			s = diroff[i];
			p = p0;
			p += s;
			if (board[p].z == player1 + 1) c1++;
			if (board[p].z == 2 - player1) c2++;
		}
		n = 0;
		if (board[p0].h[player1].ps0 < 2) n++;
		if (board[p0].h[player1].ps1 < 2) n++;
		if (board[p0].h[player1].ps2 < 2) n++;
		if (board[p0].h[player1].ps3 < 2) n++;
		if (n > 2) y -= 8;
		if (c1 + c2 == 0) y -= 20;
		if (c2 == 0 && c1 > 0 && board[p0].h[player1].pv > 9)
		{
			y += (c1 + 1) * 5;
		}
		if (board[p0].h[1 - player1].pv < 5)
		{
			n = 0;
			if (board[p0].h[player1].ps0 >= H12) n++;
			if (board[p0].h[player1].ps1 >= H12) n++;
			if (board[p0].h[player1].ps2 >= H12) n++;
			if (board[p0].h[player1].ps3 >= H12) n++;
			y += 15;
			if (n > 1) y += n * 64;
		}
		return y + board[p0].h[player1].pv;
	}
	//---------------------------------------------------------------------------
	int getEval(int p)
	{
		int a, b;
		a = getEval(0, p);
		b = getEval(1, p);
		//zkombinuj ohodnocení obou hráèù
		return a > b ? a + b / 2 : a / 2 + b;
	}
	//---------------------------------------------------------------------------
	//obrana, zkoušej táhnout na políèka z pole winMoves1
	int defend(int player1)
	{
		int p, t;
		int m, mv, mh, y, yh, Nwins, i, j;
		int player2 = 1 - player1;
		int th, thm = 0;

		dpth++;
		Nwins = UwinMoves;
		//vypoèti ohodnocení všech políèek v seznamu
		for (t = UwinMoves - 1, th = Nwins - 1; t != -1; t--, th--)
		{
			winEval[th] = getEval(player2, winMoves1[t]);
		}

		mh = m = -0x7ffe;
		for (i = 0; Nwins > 0 && i < 20; i++)
		{
			//vyber políèko s nejvìtším ohodnocením
			mv = -0x7ffe;
			for (th = Nwins - 1; th != -1; th--)
			{
				if (winEval[th] > mv) { thm = th; mv = winEval[th]; }
			}
			if (mv < 25) break;
			//vyjmi ho ze seznamu
			j = thm;
			p = winMoves1[j];
			Nwins--;
			winMoves1[j] = winMoves1[Nwins];

			board[p].z = player1 + 1;
			evaluate(p);
			y = -alfabeta(3, player2, 0, 0);
			board[p].z = 0;
			evaluate(p);
			yh = winEval[thm] + y * 20;
			if (yh > mh)
			{
				m = y;
				mh = yh;
				bestMove = p;
				if (y > 0 || y == 0 && winMove[player, player1] == 0) break;
			}
			winEval[thm] = winEval[Nwins];
		}
		if (m < 0)
		{
			//když mám prohrát, zkusím ještì zaútoèit (nìkdy to pomùže)
			t = 0;
			for (p = goodMoves[1, player1]; p != 0; p = board[p].h[player1].nxt)
			{
				if (board[p].h[player1].pv >= H30)
				{
					if (t == MwinMoves) break;
					winMoves1[t++] = p;
				}
			}
			for (t--; t >= 0; t--)
			{
				p = winMoves1[t];
				board[p].z = player1 + 1;
				evaluate(p);
				y = -alfabeta(3, player2, 0, 0);
				board[p].z = 0;
				evaluate(p);
				if (y > m)
				{
					m = y;
					bestMove = p;
					if (y >= 0) break;
				}
			}
		}
		dpth--;
		return m;
	}
	//---------------------------------------------------------------------------
	//najde políèko s nejvìtším ohodnocením
	//když je ohodnocení moc malé, vrátí 0
	int findMax(int player1)
	{
		int p, t;
		int m, r;
		int i, k;

		m = -1;
		t = 0;
		for (i = 2; i > 0 && t == 0; i--)
			for (k = 0; k < 2; k++)
				for (p = goodMoves[i, k]; p != 0; p = board[p].h[k].nxt)
				{
					r = getEval(p);
					if (r > m)
					{
						m = r;
						t = p;
					}
				}
		return t;
	}
	//---------------------------------------------------------------------------
	//zjisti, jaké bude celkové ohodnocení po nìkolika tazích
	int lookAhead(int player1)
	{
		int p;
		int y;

		if (goodMoves[3, player1] != 0) return 500; //byla nalezena výhra
		int player2 = 1 - player1;
		p = goodMoves[3, player2];
		if (p == 0 && dpth < 4) p = findMax(player1);
		if (p == 0)
		{
			return (sum[player1] - sum[player2]) / 3;
		}
		dpth++;
		board[p].z = player1 + 1;
		evaluate(p);
		y = -lookAhead(player2);
		board[p].z = 0;
		evaluate(p);
		dpth--;
		return y;
	}
	//---------------------------------------------------------------------------
	void computer1()
	{
		int p;
		int Nresults = 0;
		int m, y = 0, rnd;
		int r;
		int player1 = player, player2 = 1 - player1;

		//první tah bude uprostøed hrací plochy
		if (moves == 0)
		{
			doMove((width / 2 + 6) * height2 + height / 2 + 1);
			return;
		}
		//druhý tah dej náhodnì na nìkteré sousední políèko
		if (moves == 1)
		{
			for (; ; ) //první tah mohl být na okraji nebo v rohu !
				switch (generator.Next(0, 4))
				{
					case 0:
						if (doMove(lastMove + 1)) return;
						break;
					case 1:
						if (doMove(lastMove - 1)) return;
						break;
					case 2:
						if (doMove(lastMove + height2)) return;
						break;
					case 3:
						if (doMove(lastMove - height2)) return;
						break;
				}
		}
		lastMove = -1;

		//když už jsou ètyøi v øadì, tak táhni bez rozmýšlení
		if (doMove(goodMoves[3, player1])) return; //právì jsem vyhrál
		if (doMove(goodMoves[3, player2])) return; //musím se bránit

		//zkoušej dìlat všechny možné ètveøice
		bestMove = 0;
		if (doMove(try4(player1))) return; //urèitì vyhraju

		//co když soupeø bude dìlat jen ètveøice 
		p = try4(player2);
		if (p != 0)
		{
			//soupeø mùže vyhrát => musím se bránit 	
			winMove[player1, player2] = p;
			y = 1;
		}
		else
		{
			bestMove = 0;
			if (winMove[player1, player1] != 0)
			{
				//v pøedchozím tahu byla nalezena výhra
				y = alfabeta(1, player1, 0, winMove[player1, player1]);
				if (y <= 0) winMove[player1, player1] = 0; //soupeøi se podaøilo se ubránit
			}
			//zjisti, zda už mùžu vyhrát
			if (bestMove == 0)
			{
				y = alfabeta(3, player1, 0, 0);
			}
			if (y > 0 && bestMove != 0)
			{
				//pravdìpodobnì vyhraju
				doMove(bestMove);
				//zapamatuj si toto místo, abys v pøíštím tahu nehrál nìkde jinde
				winMove[player1, player1] = bestMove;
				return;
			}
			//zjisti, zda soupeø nemùže vyhrát
			y = 0;
			if (winMove[player1, player2] != 0)
			{
				UwinMoves = 0;
				y = alfabeta(1, player2, 1, winMove[player1, player2]);
				if (y <= 0)
				{
					winMove[player1, player2] = 0; //soupeø mohl vyhrát, ale pokazil to
				}
			}
			if (y <= 0)
			{
				UwinMoves = 0;
				y = alfabeta(3, player2, 1, 0);
				if (y > 0) winMove[player1, player2] = bestMove; //asi prohraju
			}
		}
		bestMove = 0;

		if (y > 0)
		{
			//obrana
			if (UwinMoves > 0)
			{
				//zkoušej se bránit jen na políèkách, kde byla nalezena výhra soupeøe
				defend(player1);
			}
			if (bestMove == 0)
			{
				//zkoušej se bránit kdekoli
				alfabeta(2, player1, 0, 0);
			}
		}

		if (bestMove == 0 && moves > 9)
		{
			//prohledávání do hloubky nenalezlo výherní tahy
			m = -0x7ffffffe;
			for (p = 0; p < boardk; p++)
			{
				if (board[p].z == 0 && (board[p].h[0].pv > 10 || board[p].h[1].pv > 10))
				{
					r = getEval(p);
					board[p].z = player1 + 1;
					evaluate(p);
					r -= lookAhead(player2);
					board[p].z = 0;
					evaluate(p);
					if (r > m)
					{
						m = r;
						bestMove = p;
						Nresults = 1;
					}
					else if (r > m - 20)
					{
						Nresults++;
						if (generator.Next(0, Nresults) == 0) bestMove = p;
					}
				}
			}
		}
		if (bestMove == 0)
		{
			//vyber políèko s nejvìtším ohodnocením
			m = -1;
			for (p = 0; p < boardk; p++)
			{
				if (board[p].z == 0)
				{
					r = getEval(p);
					if (r > m) m = r;
				}
			}
			//náhodnì zvol políèko, které má ohodnocení o trochu menší než nejlepší
			rnd = m / 12;
			if (rnd > 30) rnd = 30;
			Nresults = 0;
			for (p = 0; p < boardk; p++)
			{
				if (board[p].z == 0)
				{
					if (getEval(p) >= m - rnd)
					{
						Nresults++;
						if (generator.Next(0, Nresults) == 0) bestMove = p;
					}
				}
			}

		}
		//koneènì proveï svùj tah 
		doMove(bestMove);
	}
	//---------------------------------------------------------------------------
}