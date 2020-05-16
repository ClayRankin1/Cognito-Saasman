import { Lookup } from './lookup';

export interface Address {
    street: string;
    city: string;
    zip: string;
    state: Lookup;
    stateId: number;
}
