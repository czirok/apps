// CultureSelector.ts - Bootstrap head integration

export class CultureSelector {
	private readonly CULTURE_COOKIE_NAME = (window as any).BlazorCultureCookie
	private readonly CULTURE_STORAGE_KEY = (window as any).BlazorCultureStorage
	private readonly DEFAULT_CULTURE = 'en-US'
	private readonly RTL_CSS_ID = (window as any).BlazorRTLId ?? 'rtl-css'
	private readonly LTR_CSS_ID = (window as any).BlazorLTRId ?? 'ltr-css'
	private readonly RTL_CULTURES = [
		'ar',
		'arc',
		'dv',
		'fa',
		'ha',
		'he',
		'khw',
		'ks',
		'ku',
		'ps',
		'ur',
		'yi',
		'ug',
		'sd',
		'prs',
		'az',
		'nqo',
		'rhg',
		'syr',
		'ff',
	]

	private _currentCulture: string

	get _browserCulture(): string {
		return navigator.language || this.DEFAULT_CULTURE
	}

	get _cookieCulture(): string | null {
		const cookieValue = this._getCookie(this.CULTURE_COOKIE_NAME)
		if (!cookieValue)
			return null

		try {
			// Dekoding: "c%3Dhu%7Cuic%3Dhu" → "c=hu|uic=hu"
			const decoded = decodeURIComponent(cookieValue)
			// "c=hu|uic=hu" → "hu"
			const match = decoded.match(/c=([^|]+)/)
			return match ? match[1] : null
		}
		catch {
			return null
		}
	}

	get _storageCulture(): string | null {
		const storageValue = localStorage.getItem(this.CULTURE_STORAGE_KEY)
		if (!storageValue)
			return null

		try {
			// Same format as cookie: "c=hu|uic=hu" → "hu"
			const match = storageValue.match(/c=([^|]+)/)
			return match ? match[1] : null
		}
		catch {
			return null
		}
	}

	constructor() {
		if (this.CULTURE_STORAGE_KEY) {
			// Storage mode
			this._currentCulture = this._storageCulture || this._browserCulture
			this._setStorage(this._currentCulture)
		}
		else {
			// Cookie mode (original logic)
			this._currentCulture = this._cookieCulture || this._browserCulture
			this._setCookie(this._currentCulture)
		}

		// Global variable for Blazor.start()
		;(window as any).BlazorCulture = this._currentCulture

		// HTML lang and dir attributes setting + CSS activation
		this._setHtmlAttributes(this._currentCulture)
	}

	getCookie(): string {
		return this._currentCulture
	}

	setCookie(culture: string): void {
		if (culture === this._currentCulture)
			return

		this._currentCulture = culture

		if (this.CULTURE_STORAGE_KEY) {
			this._setStorage(culture)
		}
		else {
			this._setCookie(culture)
		}

		;(window as any).BlazorCulture = culture
	}

	getStorage(): string {
		return this._currentCulture
	}

	setStorage(culture: string): void {
		if (culture === this._currentCulture)
			return

		this._currentCulture = culture

		if (this.CULTURE_STORAGE_KEY) {
			this._setStorage(culture)
		}
		else {
			this._setCookie(culture)
		}

		;(window as any).BlazorCulture = culture
	}

	private _setHtmlAttributes(culture: string): void {
		// <html lang="hu"> or <html lang="en-US">
		document.documentElement.setAttribute('lang', culture)

		// <html dir="rtl"> or <html dir="ltr">
		const langCode = culture.split('-')[0]
		const isRtl = this.RTL_CULTURES.includes(langCode)
		document.documentElement.setAttribute('dir', isRtl ? 'rtl' : 'ltr')

		// CSS activation
		const cssLink = document.getElementById(isRtl ? this.RTL_CSS_ID : this.LTR_CSS_ID) as HTMLLinkElement
		if (cssLink) {
			cssLink.disabled = false
		}
	}

	private _setStorage(culture: string): void {
		// ASP.NET RequestCulture format: "c=hu|uic=hu"
		const cultureValue = `c=${culture}|uic=${culture}`
		localStorage.setItem(this.CULTURE_STORAGE_KEY, cultureValue)
	}

	private _setCookie(culture: string): void {
		// ASP.NET RequestCulture format: "c=hu|uic=hu"
		const cookieValue = `c=${culture}|uic=${culture}`
		const encodedValue = encodeURIComponent(cookieValue)

		document.cookie = `${this.CULTURE_COOKIE_NAME}=${encodedValue}`
	}

	private _getCookie(name: string): string | null {
		const nameEQ = `${name}=`
		const cookies = document.cookie.split(';')

		for (let cookie of cookies) {
			cookie = cookie.trim()
			if (cookie.indexOf(nameEQ) === 0) {
				return cookie.substring(nameEQ.length)
			}
		}
		return null
	}
}
