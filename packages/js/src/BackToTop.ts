import type { DotNet } from '@microsoft/dotnet-js-interop'

export class BackToTop {
	private dotNetObjRef: DotNet.DotNetObject | undefined
	private scrollEventHandler: () => Promise<void>

	constructor() {
		this.scrollEventHandler = async () => {
			await this.dotNetObjRef?.invokeMethodAsync('OnUpdate', window.scrollY)
		}
	}

	setReference(dotNetObjRef: DotNet.DotNetObject) {
		this.dotNetObjRef = dotNetObjRef

		window.addEventListener('scroll', this.scrollEventHandler)
		return window.scrollY
	}

	scrollTop() {
		document.body.scrollTop = 0
		document.documentElement.scrollTop = 0
	}

	dispose() {
		this.dotNetObjRef?.dispose()
		window.removeEventListener('scroll', this.scrollEventHandler)
	}
}
