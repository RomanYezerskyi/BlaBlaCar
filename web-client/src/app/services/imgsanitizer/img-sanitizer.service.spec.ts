import { TestBed } from '@angular/core/testing';

import { ImgSanitizerService } from './img-sanitizer.service';

describe('ImgSanitizerService', () => {
  let service: ImgSanitizerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImgSanitizerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
