import { BackToTop } from './BackToTop.js'
import { OffcanvasSwipe } from './OffcanvasSwipe.js'

export class BlazorBody {
	OffcanvasSwipe: OffcanvasSwipe
	BackToTop: BackToTop
	constructor() {
		this.OffcanvasSwipe = new OffcanvasSwipe()
		this.BackToTop = new BackToTop()
	}
}

(window as any).BlazorBody = new BlazorBody()
