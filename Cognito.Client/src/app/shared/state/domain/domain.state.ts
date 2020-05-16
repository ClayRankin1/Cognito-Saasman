import { State, Selector, Action, StateContext } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { DomainStateModel } from '../../models/state/domain-state.model';
import { Domain } from './domain.actions';

const defaultState: DomainStateModel = {
    id: 0,
};

@State<DomainStateModel>({
    name: 'domain',
    defaults: defaultState,
})
@Injectable()
export class DomainState {
    constructor() {}

    @Selector()
    static domainId(state: DomainStateModel): number {
        return state.id;
    }

    @Action(Domain.Edit)
    edit(ctx: StateContext<DomainStateModel>, { payload }: Domain.Edit): DomainStateModel {
        return ctx.patchState({
            id: payload.id,
        });
    }
}
