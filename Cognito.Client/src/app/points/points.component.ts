import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { AuthService } from '../auth/auth.service';
import { DxTreeListComponent, DxDraggableComponent, DxDraggableModule } from 'devextreme-angular';
import { CommonService } from '../shared/services/common.service';
import { User } from '../auth/user.model';
import { PointsService } from './points.service';
import { DetailsService } from '../details/details.service';
import notify from 'devextreme/ui/notify';
import { MenuList } from '../shared/models/menulist';
import { Point } from './point.model';
import { InsertedPoint } from './point.model';
import { dxEvent } from 'devextreme/events';
import dxDraggable, { dxDraggableOptions } from 'devextreme/ui/draggable';
import {
    DxoDragBoxStyleComponent,
    DxoRowDraggingComponent,
    DxoRowDraggingModule,
    DxoItemDraggingModule,
    DxoDragBoxStyleModule,
    DxoItemDraggingComponent,
    DxoAppointmentDraggingComponent,
    DxoAppointmentDraggingModule,
} from 'devextreme-angular/ui/nested';
import DataSource from 'devextreme/data/data_source';

@Component({
    selector: 'Points',
    templateUrl: './points.component.html',
    styleUrls: ['./points.component.scss'],
})
export class PointsComponent implements OnInit, AfterViewInit {
    @ViewChild(DxTreeListComponent)
    treeList: DxTreeListComponent;
    dataSource: CustomStore;
    menulist: MenuList[];
    currentUser: User;
    selectedItems: number[];
    isShowLinked: boolean;

    constructor(
        private auth: AuthService,
        private _commService: CommonService,
        private pointsService: PointsService,
        private items: DetailsService
    ) {
        this.currentUser = this.auth.currentUser;
        this.menulist = [
            {
                id: '1',
                name: 'Link',
                icon: '',
                items: [],
            },
        ];
    }

    ngAfterViewInit() {
        this._commService.updateListsObs.subscribe(() => {
            this.treeList.instance.refresh();
        });
    }

    ngOnInit() {
        this.dataSource = this.getDataSource();

        this.items.selectedItems$.subscribe((items) => {
            this.selectedItems = items;
        });

        this.items.showLinked$.subscribe((showLinked) => {
            this.isShowLinked = showLinked;
            this.treeList && this.treeList.instance.refresh();
        });
    }

    onDragChange = (e) => {
        let visibleRows = e.component.getVisibleRows(),
            sourceNode = e.component.getNodeByKey(e.itemData.id),
            targetNode = visibleRows[e.toIndex].node;

        while (targetNode && targetNode.data) {
            if (targetNode.data.id === sourceNode.data.id) {
                e.cancel = true;
                break;
            }
            targetNode = targetNode.parent;
        }
    };

    getDataSource = () => {
        return new CustomStore({
            key: 'id',
            remove: async (key: number) => await this.pointsService.deletePoint(key),
            update: async (key: number, value: Point) => {
                return await this.pointsService.updatePoint(key, value);
            },
            insert: async (values: InsertedPoint) => {
                return await this.pointsService.insertPoint(values);
            },
            load: async () => {
                const pointData = this.isShowLinked
                    ? await this.pointsService.getLinkedPoints(this.selectedItems)
                    : await this.pointsService.getPoints();
                return pointData;
            },
        });
    };

    onReorder = async (e: any) => {
        let visibleRows = e.component.getVisibleRows(),
            sourceData: Point = e.itemData,
            targetData: Point = visibleRows[e.toIndex].data;

        if (e.dropInsideItem) {
            e.itemData.parentId = targetData.id;
            sourceData.count = targetData.children.length + 1;
        } else {
            sourceData.parentId = targetData.parentId;
            sourceData.count = targetData.count;
        }

        await this.pointsService
            .reorderPoint(sourceData.id, sourceData)
            .then(() => e.component.refresh());
    };

    selectionChanged = (e: any) => {
        this.pointsService.updateSelectedPoints(e.selectedRowsData);
    };

    onLink = async () => {
        let selectedPoints: number[] = this.treeList.instance.getSelectedRowKeys();
        let selectedPoint: number = null;

        if (selectedPoints.length === 1) {
            selectedPoint = selectedPoints[0];
        } else {
            notify('A point must be selected to link.', 'info', 2000);
            return;
        }

        if (this.selectedItems.length === 0) {
            notify('Select one or more items to link to a point.', 'info', 2000);
            return;
        }

        await this.pointsService
            .linkPoints(selectedPoint, this.selectedItems)
            .then(() => notify('Successfully linked points', 'info', 2000));
    };

    onMenuClick = async (data: any) => {
        let item = data.itemData;
        switch (item.name) {
            case 'Link': {
                await this.onLink();
                break;
            }
            default: {
                break;
            }
        }
    };

    onToolbarPreparing(e) {
        let toolbarItems = e.toolbarOptions.items;
        let addRowButton = toolbarItems.find((x) => x.name === 'addRowButton');
        let searchPanel = toolbarItems.find((x) => x.name === 'searchPanel');
        if (addRowButton) {
            addRowButton.location = 'before';
        }
        if (searchPanel) {
            searchPanel.location = 'before';
        }

        let nToolbarItems = [];
        if (toolbarItems.length) {
            nToolbarItems[0] = toolbarItems[1];
            nToolbarItems[1] = toolbarItems[0];
        }
        e.toolbarOptions.items = nToolbarItems;
    }
}
