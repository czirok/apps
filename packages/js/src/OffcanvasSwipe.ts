export class OffcanvasSwipe {
	private static readonly MinHorizontalMove = 100
	private static readonly MaxVerticalMove = 50
	private static readonly Millisecond = 300

	private swipeStartTime: Date = new Date(0)
	private swipeStartX: number = 0
	private swipeStartY: number = 0
	private swipeStartElement?: HTMLElement
	private swipeElement?: HTMLElement
	private offcanvasElement?: HTMLElement
	private isInitialized: boolean = false
	private isTouch: boolean = false

	// Bound event handlers to enable proper removal
	private boundHandlePointerDown: (e: PointerEvent) => void
	private boundHandlePointerUp: (e: PointerEvent) => void
	private boundHandleTouchStart: (e: TouchEvent) => void
	private boundHandleTouchEnd: (e: TouchEvent) => void

	constructor() {
		// Platform support is universal, no need to check
		// Bind event handlers once to enable proper removal
		this.boundHandlePointerDown = this.handlePointerDown.bind(this)
		this.boundHandlePointerUp = this.handlePointerUp.bind(this)
		this.boundHandleTouchStart = this.handleTouchStart.bind(this)
		this.boundHandleTouchEnd = this.handleTouchEnd.bind(this)
	}

	public initialize(swipeAreaId: string, offcanvasId: string, isTouch: boolean): boolean {
		this.isTouch = isTouch

		// Element references resolution
		this.swipeElement = document.getElementById(swipeAreaId) as HTMLElement
		this.offcanvasElement = document.getElementById(offcanvasId) as HTMLElement

		if (!this.swipeElement || !this.offcanvasElement) {
			return false
		}

		this.bindEvents()
		this.isInitialized = true
		return true
	}

	private bindEvents(): void {
		if (!this.swipeElement || !this.offcanvasElement)
			return

		if (this.isTouch) {
			// Touch platform mode - TouchEvents only
			// Main area - for opening menu
			this.swipeElement.addEventListener('touchstart', this.boundHandleTouchStart)
			this.swipeElement.addEventListener('touchend', this.boundHandleTouchEnd)
			// Sidebar area - for closing menu
			this.offcanvasElement.addEventListener('touchstart', this.boundHandleTouchStart)
			this.offcanvasElement.addEventListener('touchend', this.boundHandleTouchEnd)
		}
		else {
			// Desktop platform mode - PointerEvents only
			// Main area - for opening menu
			this.swipeElement.addEventListener('pointerdown', this.boundHandlePointerDown)
			this.swipeElement.addEventListener('pointerup', this.boundHandlePointerUp)
			// Sidebar area - for closing menu
			this.offcanvasElement.addEventListener('pointerdown', this.boundHandlePointerDown)
			this.offcanvasElement.addEventListener('pointerup', this.boundHandlePointerUp)
		}
	}

	private handlePointerDown(e: PointerEvent): void {
		e.stopPropagation()
		if (!e.isPrimary)
			return

		this.startSwipe(e.clientX, e.clientY, e.currentTarget as HTMLElement)
	}

	private handlePointerUp(e: PointerEvent): void {
		e.stopPropagation()
		if (!e.isPrimary)
			return

		const moveX = e.clientX - this.swipeStartX
		const moveY = e.clientY - this.swipeStartY
		const elapsedTime = (new Date().getTime() - this.swipeStartTime.getTime())

		this.processSwipe(moveX, moveY, elapsedTime, e.currentTarget as HTMLElement)
	}

	private handleTouchStart(e: TouchEvent): void {
		e.stopPropagation()
		if (!e.touches || e.touches.length === 0)
			return

		this.startSwipe(e.touches[0].clientX, e.touches[0].clientY, e.currentTarget as HTMLElement)
	}

	private handleTouchEnd(e: TouchEvent): void {
		e.stopPropagation()
		if (!e.changedTouches || e.changedTouches.length === 0)
			return

		const moveX = e.changedTouches[0].clientX - this.swipeStartX
		const moveY = e.changedTouches[0].clientY - this.swipeStartY
		const elapsedTime = (new Date().getTime() - this.swipeStartTime.getTime())

		this.processSwipe(moveX, moveY, elapsedTime, e.currentTarget as HTMLElement)
	}

	private startSwipe(x: number, y: number, element: HTMLElement): void {
		this.swipeStartX = x
		this.swipeStartY = y
		this.swipeStartTime = new Date()
		this.swipeStartElement = element
	}

	private processSwipe(moveX: number, moveY: number, elapsedTime: number, currentElement: HTMLElement): void {
		if (Math.abs(moveX) > OffcanvasSwipe.MinHorizontalMove
			&& Math.abs(moveY) < OffcanvasSwipe.MaxVerticalMove
			&& elapsedTime < OffcanvasSwipe.Millisecond) {
			const angle = Math.abs(Math.atan2(moveY, moveX) * 180 / Math.PI)
			if (angle <= 45 || angle >= 135) {
				// Check if swipe started and ended on the same element
				if (this.swipeStartElement !== currentElement) {
					return
				}

				const isMainArea = currentElement === this.swipeElement
				const isSidebarArea = currentElement === this.offcanvasElement

				const isRTL = document.documentElement.dir === 'rtl' || document.dir === 'rtl'

				if (isMainArea) {
					const shouldOpen = isRTL ? moveX < 0 : moveX > 0
					if (shouldOpen) {
						this.showMenu()
					}
				}
				else if (isSidebarArea) {
					const shouldClose = isRTL ? moveX > 0 : moveX < 0
					if (shouldClose) {
						this.hideMenu()
					}
				}
			}
		}
	}

	private hideMenu(): void {
		if (!this.offcanvasElement)
			return

		if (this.isOffcanvasActive(this.offcanvasElement)) {
			const offcanvasInstance = (window as any).bootstrap?.Offcanvas?.getInstance(this.offcanvasElement)
			if (offcanvasInstance) {
				offcanvasInstance.hide()
			}
			else {
				this.offcanvasElement.classList.remove('show')
			}
		}
	}

	private showMenu(): void {
		if (!this.offcanvasElement)
			return

		if (this.isOffcanvasActive(this.offcanvasElement)) {
			const offcanvasInstance = (window as any).bootstrap?.Offcanvas?.getInstance(this.offcanvasElement)
				|| new (window as any).bootstrap.Offcanvas(this.offcanvasElement)
			if (offcanvasInstance) {
				offcanvasInstance.show()
			}
			else {
				this.offcanvasElement.classList.add('show')
			}
		}
	}

	private isOffcanvasActive(offcanvasElement: HTMLElement): boolean {
		const classList = offcanvasElement.classList
		const breakpoints = ['sm', 'md', 'lg', 'xl', 'xxl']

		const hasOffcanvasBreakpoint = breakpoints.some(bp =>
			classList.contains(`offcanvas-${bp}`),
		)

		if (!hasOffcanvasBreakpoint && classList.contains('offcanvas')) {
			return true
		}

		if (hasOffcanvasBreakpoint) {
			const viewportWidth = window.innerWidth
			const breakpointValues: { [key: string]: number } = {
				sm: 576,
				md: 768,
				lg: 992,
				xl: 1200,
				xxl: 1400,
			}

			for (const bp of breakpoints) {
				if (classList.contains(`offcanvas-${bp}`)) {
					const isActive = viewportWidth < breakpointValues[bp]
					return isActive
				}
			}
		}

		return false
	}

	public dispose(): void {
		if (this.isInitialized) {
			if (this.isTouch) {
				// Touch platform mode - remove TouchEvents
				if (this.swipeElement) {
					this.swipeElement.removeEventListener('touchstart', this.boundHandleTouchStart)
					this.swipeElement.removeEventListener('touchend', this.boundHandleTouchEnd)
				}
				if (this.offcanvasElement) {
					this.offcanvasElement.removeEventListener('touchstart', this.boundHandleTouchStart)
					this.offcanvasElement.removeEventListener('touchend', this.boundHandleTouchEnd)
				}
			}
			else {
				// Desktop platform mode - remove PointerEvents
				if (this.swipeElement) {
					this.swipeElement.removeEventListener('pointerdown', this.boundHandlePointerDown)
					this.swipeElement.removeEventListener('pointerup', this.boundHandlePointerUp)
				}
				if (this.offcanvasElement) {
					this.offcanvasElement.removeEventListener('pointerdown', this.boundHandlePointerDown)
					this.offcanvasElement.removeEventListener('pointerup', this.boundHandlePointerUp)
				}
			}
			this.isInitialized = false
		}
	}
}
