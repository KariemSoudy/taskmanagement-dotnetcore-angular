import { TestBed, inject } from '@angular/core/testing';

import { TasksServiceService } from './tasks-service.service';

describe('TasksServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TasksServiceService]
    });
  });

  it('should be created', inject([TasksServiceService], (service: TasksServiceService) => {
    expect(service).toBeTruthy();
  }));
});
