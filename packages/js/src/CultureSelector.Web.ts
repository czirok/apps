export class CultureSelector {
	private readonly CULTURE_COOKIE_NAME = (window as any).BlazorCultureCookie
	private readonly DEFAULT_CULTURE = 'en-US'

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

	constructor() {
		this._currentCulture = this._cookieCulture || this._browserCulture
		this._setCookie(this._currentCulture)

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

		this._setCookie(culture)

		;(window as any).BlazorCulture = culture
	}

	private _setHtmlAttributes(culture: string): void {
		// <html lang="hu"> or <html lang="en-US">
		document.documentElement.setAttribute('lang', culture)
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
