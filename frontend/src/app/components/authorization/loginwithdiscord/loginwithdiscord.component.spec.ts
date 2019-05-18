import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginwithdiscordComponent } from './loginwithdiscord.component';

describe('LoginwithdiscordComponent', () => {
  let component: LoginwithdiscordComponent;
  let fixture: ComponentFixture<LoginwithdiscordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoginwithdiscordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginwithdiscordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
