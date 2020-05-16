import { Component, HostBinding } from '@angular/core';
import { ScreenService } from '../../shared/services';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Subject } from 'rxjs';

@Component({
    templateUrl: 'home.component.html',
    selector: 'home',
    styleUrls: ['home.component.scss'],
})
export class HomeComponent {
    istasksPressed = true;
    isitemsPressed = true;
    istabsPressed = true;
    tasksthree = true;
    taskstwo = false;
    tasksone = false;
    itemsthree = true;
    itemstwo = false;
    itemsone = false;
    tabsthree = true;
    tabstwo = false;
    tabsone = false;
    webUrl: SafeResourceUrl = '';
    isClicked = false;
    gridClear: Subject<void> = new Subject<void>();

    @HostBinding('class') get getClass() {
        return Object.keys(this.screen.sizes)
            .filter((cl) => this.screen.sizes[cl])
            .join(' ');
    }
    panellayout: number = 1;

    constructor(private screen: ScreenService, public sanitizer: DomSanitizer) { }
    ngOnInit() {
        this.panellayout = 0;
    }

    contentReady(e) {
        e.component.option('selectedIndex', localStorage.getItem('selectedtabindex'));
    }
    selectionChanged(e) {
        let selectedtabindex: number;
        switch (e.addedItems[0].title) {
            case 'Points': {
                selectedtabindex = 0;
                break;
            }
            case 'Documents': {
                selectedtabindex = 1;
                break;
            }
            case 'Contacts': {
                selectedtabindex = 2;
                break;
            }
            case 'WebSites': {
                selectedtabindex = 3;
                break;
            }
            default: {
                selectedtabindex = 0;
                break;
            }
        }
        localStorage.setItem('selectedtabindex', selectedtabindex.toString());
    }

    panel1Click() {
        this.panellayout = 1;
    }
    panel2Click() {
        this.panellayout = 2;
    }
    panel3Click() {
        this.panellayout = 3;
    }

    handleTasks() {
        this.istasksPressed = !this.istasksPressed;
        if (this.istasksPressed) {
            if (this.isitemsPressed && !this.istabsPressed) {
                this.taskstwo = true;
                this.tasksthree = false;
                this.tasksone = false;

                this.itemstwo = true;
                this.itemsone = false;
                this.itemsthree = false;
            }
            if (!this.isitemsPressed && this.istabsPressed) {
                this.taskstwo = true;
                this.tasksthree = false;
                this.tasksone = false;

                this.tabstwo = true;
                this.tabsone = false;
                this.tabsthree = false;
            }
            if (this.isitemsPressed && this.istabsPressed) {
                this.tasksthree = true;
                this.taskstwo = false;
                this.tasksone = false;

                this.itemstwo = false;
                this.itemsone = false;
                this.itemsthree = true;

                this.tabstwo = false;
                this.tabsone = false;
                this.tabsthree = true;
            }
            if (!this.isitemsPressed && !this.istabsPressed) {
                this.tasksone = true;
                this.taskstwo = false;
                this.tasksthree = false;
            }
        } else {
            if (this.isitemsPressed && !this.istabsPressed) {
                this.itemsone = true;
                this.itemstwo = false;
                this.itemsthree = false;
            }
            if (!this.isitemsPressed && this.istabsPressed) {
                this.tabsone = true;
                this.tabstwo = false;
                this.tabsthree = false;
            }
            if (this.isitemsPressed && this.istabsPressed) {
                this.itemstwo = true;
                this.itemsone = false;
                this.itemsthree = false;
                this.tabstwo = true;
                this.tabsone = false;
                this.tabsthree = false;
            }
        }
    }
    handleItems() {
        this.isitemsPressed = !this.isitemsPressed;
        if (this.isitemsPressed) {
            if (this.istasksPressed && !this.istabsPressed) {
                this.itemstwo = true;
                this.itemsthree = false;
                this.itemsone = false;

                this.tasksthree = false;
                this.taskstwo = true;
                this.tasksone = false;
            }
            if (!this.istasksPressed && this.istabsPressed) {
                this.itemstwo = true;
                this.itemsthree = false;
                this.itemsone = false;

                this.tabsone = false;
                this.tabstwo = true;
                this.tabsthree = false;
            }
            if (this.istasksPressed && this.istabsPressed) {
                this.tasksthree = true;
                this.taskstwo = false;
                this.tasksone = false;

                this.itemstwo = false;
                this.itemsone = false;
                this.itemsthree = true;

                this.tabstwo = false;
                this.tabsone = false;
                this.tabsthree = true;
            }
            if (!this.istasksPressed && !this.istabsPressed) {
                this.itemsone = true;
                this.itemstwo = false;
                this.itemsthree = false;
            }
        } else {
            if (this.istasksPressed && !this.istabsPressed) {
                this.tasksone = true;
                this.taskstwo = false;
                this.tasksthree = false;
            }
            if (!this.istasksPressed && this.istabsPressed) {
                this.tabsone = true;
                this.tabstwo = false;
                this.tabsthree = false;
            }
            if (this.istasksPressed && this.istabsPressed) {
                this.taskstwo = true;
                this.tasksone = false;
                this.tasksthree = false;
                this.tabstwo = true;
                this.tabsone = false;
                this.tabsthree = false;
            }
        }
    }
    handleTabs() {
        this.istabsPressed = !this.istabsPressed;

        if (this.istabsPressed) {
            if (this.istasksPressed && !this.isitemsPressed) {
                this.tabstwo = true;
                this.tabsthree = false;
                this.tabsone = false;

                this.tasksthree = false;
                this.taskstwo = true;
                this.tasksone = false;
            }
            if (!this.istasksPressed && this.isitemsPressed) {
                this.itemstwo = true;
                this.itemsthree = false;
                this.itemsone = false;

                this.tabsone = false;
                this.tabstwo = true;
                this.tabsthree = false;
            }
            if (this.istasksPressed && this.isitemsPressed) {
                this.tasksthree = true;
                this.taskstwo = false;
                this.tasksone = false;

                this.itemstwo = false;
                this.itemsone = false;
                this.itemsthree = true;

                this.tabstwo = false;
                this.tabsone = false;
                this.tabsthree = true;
            }
            if (!this.istasksPressed && !this.isitemsPressed) {
                this.tabstwo = false;
                this.tabsone = true;
                this.tabsthree = false;
            }
        } else {
            if (this.istasksPressed && !this.isitemsPressed) {
                this.tasksone = true;
                this.taskstwo = false;
                this.tasksthree = false;
            }
            if (!this.istasksPressed && this.isitemsPressed) {
                this.itemstwo = false;
                this.itemsone = true;
                this.itemsthree = false;
            }
            if (this.istasksPressed && this.isitemsPressed) {
                this.taskstwo = true;
                this.tasksone = false;
                this.tasksthree = false;

                this.itemstwo = true;
                this.itemsone = false;
                this.itemsthree = false;
            }
        }
    }

    onOpen(url) {
        this.webUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
        this.isClicked = true;
    }

    gridCleaFromTask() {
        this.gridClear.next();
    }
}
