import { ChangeTheme } from './ChangeTheme.js'
import { CultureSelector } from './CultureSelector.js'

export class BlazorHead {
	ChangeTheme: ChangeTheme
	CultureSelector: CultureSelector
	constructor() {
		this.ChangeTheme = new ChangeTheme()
		this.CultureSelector = new CultureSelector()
	}
}

(window as any).BlazorHead = new BlazorHead()
