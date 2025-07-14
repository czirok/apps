// ChangeTheme.ts

import type { DotNet } from '@microsoft/dotnet-js-interop'

export class ChangeTheme {
	private readonly THEME_STORAGE_NAME = 'theme'
	private readonly THEME_ATTRIBUTE = 'data-bs-theme'
	private readonly THEME_LIGHT = 'light'
	private readonly THEME_DARK = 'dark'
	private readonly THEME_AUTO = 'auto'

	private dotNetObjRef?: DotNet.DotNetObject
	private _darkColorScheme: MediaQueryList

	get _systemTheme(): string {
		return this._darkColorScheme.matches ? this.THEME_DARK : this.THEME_LIGHT
	}

	get _storageTheme(): string | null {
		return localStorage.getItem(this.THEME_STORAGE_NAME)
	}

	get _htmlTheme(): string | null {
		return document.documentElement.getAttribute(this.THEME_ATTRIBUTE)
	}

	constructor() {
		this._darkColorScheme = window.matchMedia('(prefers-color-scheme: dark)')
		this._keepYourEyesOnTheTheme()
	}

	setReference(dotNetObjRef: DotNet.DotNetObject): void {
		this.dotNetObjRef = dotNetObjRef
	}

	getTheme(): string | null {
		return this._storageTheme
	}

	setTheme(theme: string): void {
		if (theme === this.THEME_AUTO)
			this._setAllAndInvokeBlazor(theme, this._systemTheme, this.THEME_AUTO)
		else
			this._setAllAndInvokeBlazor(theme, theme, theme)
	}

	_keepYourEyesOnTheTheme(): void {
		this._htmlElementChanged()

		const htmlElementChanged = (mutationList: MutationRecord[]) => {
			for (const mutation of mutationList) {
				if (mutation.type === 'attributes' && mutation.attributeName === this.THEME_ATTRIBUTE)
					this._htmlElementChanged()
			}
		}

		const observer = new MutationObserver(htmlElementChanged)
		observer.observe(document.documentElement, {
			attributes: true,
			childList: false,
			characterData: false,
		})

		const systemThemeChanged = async (dark: MediaQueryListEvent) => {
			if (dark.matches)
				await this._systemThemeChanged(this.THEME_DARK)
			else
				await this._systemThemeChanged(this.THEME_LIGHT)
		}

		this._darkColorScheme.addEventListener('change', systemThemeChanged)
	}

	_htmlElementChanged(): void {
		if (this._storageTheme) {
			if (this._storageTheme === this.THEME_AUTO) {
				if (this._htmlTheme !== this._systemTheme)
					this._setHtmlAndInvokeBlazor(this._systemTheme, this.THEME_AUTO)
			}
			else if (this._storageTheme !== this._htmlTheme) {
				this._setHtmlAndInvokeBlazor(this._storageTheme, this._storageTheme)
			}
		}
		else {
			this._setAllAndInvokeBlazor(this.THEME_AUTO, this._systemTheme, this.THEME_AUTO)
		}
	}

	async _systemThemeChanged(newValue: string): Promise<void> {
		if (this._storageTheme) {
			if (this._storageTheme === this.THEME_AUTO && this._htmlTheme !== newValue)
				await this._setHtmlAndInvokeBlazor(newValue, this.THEME_AUTO)
		}
		else {
			await this._setAllAndInvokeBlazor(this.THEME_AUTO, this._systemTheme, this.THEME_AUTO)
		}
	}

	async _setHtmlAndInvokeBlazor(html: string, theme: string): Promise<void> {
		document.documentElement.setAttribute(this.THEME_ATTRIBUTE, html)
		await this.dotNetObjRef?.invokeMethodAsync('OnUpdate', theme)
	}

	async _setAllAndInvokeBlazor(storage: string, html: string, theme: string): Promise<void> {
		document.documentElement.setAttribute(this.THEME_ATTRIBUTE, html)
		localStorage.setItem(this.THEME_STORAGE_NAME, storage)
		await this.dotNetObjRef?.invokeMethodAsync('OnUpdate', theme)
	}
}
