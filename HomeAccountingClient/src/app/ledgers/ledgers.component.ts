import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { LedgersService } from '../shared/services/ledgers.service';
import { LedgerResponseDto } from '../_interfaces/Legder/LedgerResponseDto';

@Component({
  selector: 'app-ledgers',
  templateUrl: './ledgers.component.html',
  styleUrls: ['./ledgers.component.css']
})
export class LedgersComponent implements OnInit {

  constructor(private ledgService : LedgersService) { }

  ledgers : LedgerResponseDto[];
  ledgersForm : FormGroup;

  ngOnInit(): void {
    this.loadLedgers();
    this.ledgersForm = this.ledgService.initLedgerForm()
  }

  loadLedgers(){
    this.ledgService.getLedgers().subscribe((response : any) => {
      this.ledgers = response.data;
      console.log(this.ledgers);
    })
  }

}
