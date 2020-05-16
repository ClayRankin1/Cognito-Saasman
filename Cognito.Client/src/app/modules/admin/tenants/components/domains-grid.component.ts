import { DxDataGridEvents } from 'src/app/shared/devexpress/constants/dx-data-grid-events';
import { Component, Input, OnInit } from '@angular/core';
import { DxDataGridHelper } from 'src/app/shared/devexpress/helpers/dx-data-grid-helper';
import { tap } from 'rxjs/operators';
import { createGridDataSource } from 'src/app/shared/data-sources/data-sources';
import DataSource from 'devextreme/data/data_source';
import { UserRoles } from 'src/app/auth/user.roles.enum';
import { DxFormItemEditorOptions } from 'src/app/shared/devexpress/constants/dx-form-item-editor-options';
import { GridRowEvent } from 'src/app/shared/devexpress/event-models/grid-row-event';
import { DomainViewModel } from '../models/domain-view-model';
import { License, LicenseType, DomainLicense } from 'src/app/data/schema';
import { DomainService, TenantService, LicenseService } from 'src/app/data/service';
import { UserDomainsService } from 'src/app/shared/services/user-domains.service';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
    selector: 'app-domains-grid',
    templateUrl: './domains-grid.component.html',
})
export class DomainsGridComponent implements OnInit {
    @Input() licenses: License[];
    @Input() tenantRow: any;
    @Input() gridId: string;
    editorOptions = DxFormItemEditorOptions;
    isSysAdmin = false;

    private shouldShowDomainAdminForm = false;
    private dataSource: DataSource;
    constructor(
        private authService: AuthService,
        private licenseService: LicenseService,
        private domainService: DomainService,
        private tenantService: TenantService,
        private userDomainsService: UserDomainsService
    ) {}

    ngOnInit(): void {
        this.isSysAdmin = this.authService.isInRole(UserRoles.SysAdmin);
    }

    getGridAttributes = (): any => ({ id: this.gridId, style: 'max-height:500px' });

    getDataSource(): DataSource {
        if (!this.dataSource) {
            this.dataSource = createGridDataSource(
                { key: 'id', entityName: 'Domain' },
                this.domainService,
                () =>
                    this.tenantService.getDomainsByTenantId(this.tenantRow.key).pipe(
                        tap((domains: DomainViewModel[]) => {
                            for (const domain of domains) {
                                const basicLicense = domain.domainLicenses.find(
                                    (d) => d.license.licenseTypeId === LicenseType.Basic
                                );
                                const timekeeperLicense = domain.domainLicenses.find(
                                    (d) => d.license.licenseTypeId === LicenseType.TimeKeeper
                                );

                                domain.basicLicense = basicLicense;
                                domain.timekeeperLicense = timekeeperLicense;
                                domain.domainAdmins = this.userDomainsService.getUserEmailsByRole(
                                    domain.userDomains,
                                    UserRoles.DomainAdmin
                                );
                            }
                        })
                    )
            );
        }
        return this.dataSource;
    }

    onGridInitialized(e: any): void {
        new DxDataGridHelper(e.component)
            .sendAllFieldsOnUpdate()
            .setPopupTitle('Add Domain', 'Edit Domain')
            .fillMasterDataObject('tenant', this.tenantRow)
            .fillMasterKey('tenantId', this.tenantRow)
            .hideToolbar()
            .applyRowNavigationBehavior()
            .resizeOnContentReady();

        e.component.on(DxDataGridEvents.RowUpdating, this.onGridRowUpdating.bind(this));
    }

    onGridCustomizeItem = (item: any): void => {
        if (item.name === 'domainAdmin') {
            item.visible = this.shouldShowDomainAdminForm;
        }
    };

    onGridRowInserting(e: GridRowEvent<DomainViewModel>): void {
        e.data.domainLicenses = [e.data.basicLicense, e.data.timekeeperLicense];
        e.data.basicLicense = null;
        e.data.timekeeperLicense = null;
    }

    onGridRowUpdating(e: GridRowEvent<DomainViewModel>): void {
        e.newData.domainLicenses = [e.newData.basicLicense, e.newData.timekeeperLicense];
        e.newData.basicLicense = null;
        e.newData.timekeeperLicense = null;
    }

    onGridEditingStart(): void {
        this.shouldShowDomainAdminForm = false;
    }

    onGridInitNewRow(e: GridRowEvent<DomainViewModel>): void {
        const basicLicense = this.licenseService.getLicenseByType(this.licenses, LicenseType.Basic);
        const timekeeperLicense = this.licenseService.getLicenseByType(
            this.licenses,
            LicenseType.TimeKeeper
        );

        const basicDomainLicense: DomainLicense = {
            licenseId: basicLicense.id,
            discount: 0,
            licenses: 0,
            price: basicLicense?.price ?? 0,
        };

        const timekeeperDomainLicense: DomainLicense = {
            licenseId: timekeeperLicense.id,
            discount: 0,
            licenses: 0,
            price: timekeeperLicense?.price ?? 0,
        };

        e.data.basicLicense = basicDomainLicense;
        e.data.timekeeperLicense = timekeeperDomainLicense;
        this.shouldShowDomainAdminForm = true;
    }
}
