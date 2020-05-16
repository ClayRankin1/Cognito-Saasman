import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { createGridDataSource } from 'src/app/shared/data-sources/data-sources';
import { UserRoles } from 'src/app/auth/user.roles.enum';
import { AuthService } from 'src/app/auth/auth.service';
import DataGrid from 'devextreme/ui/data_grid';
import { DxDataGridHelper } from 'src/app/shared/devexpress/helpers/dx-data-grid-helper';
import { Lookup, License, LookupType } from 'src/app/data/schema';
import { TenantService, LookupService, LicenseService } from 'src/app/data/service';
import { DxFormItemEditorOptions } from 'src/app/shared/devexpress/constants/dx-form-item-editor-options';

@Component({
    selector: 'app-tenants',
    templateUrl: './tenants.component.html',
    styleUrls: ['./tenants.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class TenantsComponent implements OnInit {
    dataSource: any;
    states: Lookup[];
    licenses: License[];
    isSysAdmin = false;
    editorOptions = DxFormItemEditorOptions;
    private domainGridIdPrefix = 'DomainGrid';

    constructor(
        private tenantService: TenantService,
        private lookupService: LookupService,
        private licenseService: LicenseService,
        private authService: AuthService
    ) {}

    ngOnInit(): void {
        this.isSysAdmin = this.authService.isInRole(UserRoles.SysAdmin);
        this.dataSource = createGridDataSource(
            { key: 'id', entityName: 'Tenant' },
            this.tenantService
        );
        this.lookupService.getLookup(LookupType.State).subscribe((lookup) => {
            this.states = lookup;
        });

        if (this.isSysAdmin) {
            this.licenseService.getAll().subscribe((licenses) => {
                this.licenses = licenses;
            });
        }
    }

    onTenantGridInitialized(e: any): void {
        new DxDataGridHelper(e.component)
            .expandRowOnClick()
            .oneRowExpandedAtaTime()
            .sendAllFieldsOnUpdate()
            .setPopupTitle('Add Tenant', 'Edit Tenant')
            .setToolbarAddRecordButtonHint('Add New Tenant')
            .replaceEmptyStringWithNull('tenantName')
            .applyRowNavigationBehavior();
    }

    addDomainRow = (e: any): void => {
        if (!e.row.isExpanded) {
            e.component.expandRow(e.row.key);
        }

        const element = document.getElementById(this.getDomainsGridId(e.row));
        const instance = DataGrid.getInstance(element) as DataGrid;
        instance.addRow();
    };

    getDomainsGridId(tenantRow: any): string {
        return this.domainGridIdPrefix + tenantRow.key;
    }
}
