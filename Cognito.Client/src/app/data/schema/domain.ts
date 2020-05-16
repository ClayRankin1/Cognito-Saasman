import { Tenant } from './tenant';
import { DomainLicense } from './domain-license';
import { UserDomain } from './user-domain';

export class Domain {
    id: number;
    name: string;
    basicLicenses: number;
    timeKeeperLicenses: number;
    companyInformation: string;
    adminFirstName: string;
    adminLastName: string;
    adminEmail: string;
    tenant: Tenant;
    domainLicenses: DomainLicense[];
    userDomains: UserDomain[];
}
