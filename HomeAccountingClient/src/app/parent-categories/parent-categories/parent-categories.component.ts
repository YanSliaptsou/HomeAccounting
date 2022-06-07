import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ParentCategoryAddEditService } from 'src/app/shared/services/parent-category-add-edit.service';
import { ParentCategoryReceive } from 'src/app/_interfaces/ParentCategory/ParentCategoryReceiveDto';
import { ParentCategorySendDto } from 'src/app/_interfaces/ParentCategory/ParentCategorySendDto';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-parent-categories',
  templateUrl: './parent-categories.component.html',
  styleUrls: ['./parent-categories.component.css']
})
export class ParentCategoriesComponent implements OnInit {

  parentCategoryForm: FormGroup;
  parentCategories : ParentCategoryReceive[];
  parentCategory : ParentCategorySendDto = null;
  parentCategoryReceive : ParentCategoryReceive;
  parentCategotyName: string = "";
  isAdding = true;
  isEditing = false;
  showSuccess = false;
  showError = false;
  errorMessage : string;
  successMessage : string;
  modalRef?: BsModalRef;
  message?: string;

  constructor(private parentCategoryAddEditService : ParentCategoryAddEditService, private modalService: BsModalService) { }

  openModal(template: TemplateRef<any>, id : number) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
    this.getParentCategory(null, id);
  }

  ngOnInit(): void {
    this.loadParentCategories();
    this.parentCategoryForm = new FormGroup({
      categoryName : new FormControl("", [Validators.required])
    })
  }

  validateControl = (controlName: string) => {
    return this.parentCategoryForm.get(controlName).invalid && this.parentCategoryForm!.get(controlName).touched
  }

  hasError = (controlName: string, errorName: string) => {
    return this.parentCategoryForm.get(controlName).hasError(errorName)
  }

  loadParentCategories(){
    this.parentCategoryAddEditService.getParentCategories("api/parent-categories")
        .subscribe((response: ParentCategoryReceive[]) => {
          this.parentCategories = response;
        })
  }

  getParentCategory(parentCategoryForm : any, id : number){
    this.parentCategoryAddEditService.getConcreteParentCategory("api/parent-categories/" + id)
    .subscribe((response : ParentCategoryReceive) => {
      this.parentCategoryReceive = response;
      if (parentCategoryForm !== null){
      const catForm = {... parentCategoryForm}
      this.parentCategotyName = response.name;
      this.isEditing = true;
      this.isAdding = false;
      this.showSuccess = true;
      this.showError = false;
      this.successMessage = "Category nmb. " + response.id + " " + response.name + " has been selected to edit"
      }
    },error => {
      this.showError = true;
      this.showSuccess = false;
      this.errorMessage = error;
    })
  }

  addParentCategory(parentCategoryForm : any){
    const catForm = {... parentCategoryForm}
    const parentCategory : ParentCategorySendDto = {
      name : catForm.categoryName
    }
    this.parentCategoryAddEditService.addParentCategory("api/parent-categories", parentCategory)
    .subscribe((response : any) => {
      this.showSuccess = true;
      this.showError = false;
      this.successMessage = "Category " + parentCategory.name + " has been added successfuly!"
      this.loadParentCategories();
    }, error => {
      this.showError = true;
      this.showSuccess = false;
      this.errorMessage = error;
    })
  }

  editParentCategory(parentCategoryForm : any, id: number){
    this.isEditing = false;
    this.isAdding = true;
    const catForm = {... parentCategoryForm}
    const parentCategory : ParentCategorySendDto = {
      name : catForm.categoryName
    }
    this.parentCategoryAddEditService.editParentCategory("api/parent-categories/" + id, parentCategory)
    .subscribe((resp : any) => {
      this.showSuccess = true;
      this.showError = false;
      this.successMessage = "Category " + parentCategory.name + " has been edited successfuly!"
      this.loadParentCategories();
    }, error => {
      this.showError = true;
      this.showSuccess = false;
      this.errorMessage = error;
    })
  }

  deleteParentCategory(id : number){
    
    this.parentCategoryAddEditService.deleteParentCategory("api/parent-categories/" + id)
        .subscribe((response : any) => {
          this.showSuccess = true;
          this.showError = false;
          this.successMessage = "Category nmb. " + id + " has been deleted successfuly!"
          this.loadParentCategories();
        }, error => {
          this.showError = true;
        this.showSuccess = false;
        this.errorMessage = error;
        })
        this.modalRef?.hide();
  }

  decline(): void {
    this.modalRef?.hide();
  }

}
