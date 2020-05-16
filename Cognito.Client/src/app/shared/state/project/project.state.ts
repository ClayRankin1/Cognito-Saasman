import { State, Selector, Action, StateContext } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { ProjectStateModel } from '../../models/state/project-state.model';
import { Project } from './project.actions';

const defaultState: ProjectStateModel = {
    id: 0,
};

@State<ProjectStateModel>({
    name: 'project',
    defaults: defaultState,
})
@Injectable()
export class ProjectState {
    constructor() {}

    @Selector()
    static projectId(state: ProjectStateModel): number {
        return state.id;
    }

    @Action(Project.Edit)
    edit(ctx: StateContext<ProjectStateModel>, { payload }: Project.Edit): ProjectStateModel {
        return ctx.patchState({
            id: payload.id,
        });
    }
}
