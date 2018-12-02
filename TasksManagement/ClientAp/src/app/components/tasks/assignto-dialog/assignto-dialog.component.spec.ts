import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssigntoDialogComponent } from './assignto-dialog.component';

describe('AssigntoDialogComponent', () => {
  let component: AssigntoDialogComponent;
  let fixture: ComponentFixture<AssigntoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssigntoDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssigntoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
