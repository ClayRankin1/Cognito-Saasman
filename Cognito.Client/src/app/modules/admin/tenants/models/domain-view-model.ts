import { DomainLicense } from 'src/app/data/schema/domain-license';
import { Domain } from 'src/app/data/schema/domain';

export interface DomainViewModel extends Domain {
    basicLicense: DomainLicense;
    timekeeperLicense: DomainLicense;
    domainAdmins: string;
}
