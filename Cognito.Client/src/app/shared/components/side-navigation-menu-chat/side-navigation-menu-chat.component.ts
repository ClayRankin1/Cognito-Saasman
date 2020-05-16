import {
    Component,
    Output,
    Input,
    EventEmitter,
    ViewChild,
    ElementRef,
    AfterViewInit,
    OnDestroy,
} from '@angular/core';
import { DxTreeViewComponent } from 'devextreme-angular/ui/tree-view';
import * as events from 'devextreme/events';

@Component({
    selector: 'app-side-navigation-menu-chat',
    templateUrl: './side-navigation-menu-chat.component.html',
    styleUrls: ['./side-navigation-menu-chat.component.scss'],
})
export class SideNavigationMenuChatComponent implements AfterViewInit, OnDestroy {
    @ViewChild(DxTreeViewComponent, { static: true })
    menu: DxTreeViewComponent;

    @Output()
    selectedItemChanged = new EventEmitter<string>();

    @Output()
    @Input()
    itemsChat: string[];

    @Input()
    set selectedItem(value: String) {
        if (this.menu.instance) {
            this.menu.instance.selectItem(value);
        }
    }

    private _compactMode = false;
    @Input()
    get compactMode() {
        return this._compactMode;
    }
    set compactMode(val) {
        this._compactMode = val;
        if (val && this.menu.instance) {
            this.menu.instance.collapseAll();
        }
    }

    constructor(private elementRef: ElementRef) {}

    updateSelection(event) {
        const nodeClass = 'dx-treeview-node';
        const selectedClass = 'dx-state-selected';
        const leafNodeClass = 'dx-treeview-node-is-leaf';
        const element: HTMLElement = event.element;

        const rootNodes = element.querySelectorAll(`.${nodeClass}:not(.${leafNodeClass})`);
        Array.from(rootNodes).forEach((node) => {
            node.classList.remove(selectedClass);
        });

        let selectedNode: HTMLElement = element.querySelector(`.${nodeClass}.${selectedClass}`);
        while (selectedNode && selectedNode.parentElement) {
            if (selectedNode.classList.contains(nodeClass)) {
                selectedNode.classList.add(selectedClass);
            }

            selectedNode = selectedNode.parentElement;
        }
    }

    onItemClick(event) {
        this.selectedItemChanged.emit(event);
    }

    onMenuInitialized(event) {
        event.component.option('deferRendering', false);
    }

    ngAfterViewInit() {
        events.on(this.elementRef.nativeElement, 'dxclick', (e) => {});
    }

    ngOnDestroy() {
        events.off(this.elementRef.nativeElement, 'dxclick');
    }
}
