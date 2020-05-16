import { Output, Injectable, EventEmitter, Directive } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

@Directive()
@Injectable({
    providedIn: 'root',
})
export class ScreenService {
    @Output() changed = new EventEmitter();

    constructor(private breakpointObserver: BreakpointObserver) {
        this.breakpointObserver
            .observe([Breakpoints.XSmall, Breakpoints.Small, Breakpoints.Medium, Breakpoints.Large])
            .subscribe(() => this.changed.next());
    }

    private isLargeScreen() {
        const isLarge = this.breakpointObserver.isMatched(Breakpoints.Large);
        const isXLarge = this.breakpointObserver.isMatched(Breakpoints.XLarge);

        return isLarge || isXLarge;
    }

    public get sizes() {
        return {
            'screen-x-small': this.breakpointObserver.isMatched(Breakpoints.XSmall),
            'screen-small': this.breakpointObserver.isMatched(Breakpoints.Small),
            'screen-medium': this.breakpointObserver.isMatched(Breakpoints.Medium),
            'screen-large': this.isLargeScreen(),
        };
    }
}