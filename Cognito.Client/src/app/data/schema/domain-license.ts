import { License } from './license';

export class DomainLicense {
    licenseId: number;
    license?: License;
    price: number;
    discount: number;
    licenses: number;
}
