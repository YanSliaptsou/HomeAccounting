import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ParentCategoryAddEditService } from 'src/app/shared/services/parent-category-add-edit.service';
import { ParentCategoryReceive } from 'src/app/_interfaces/ParentCategory/ParentCategoryReceiveDto';
import { ParentCategorySendDto } from 'src/app/_interfaces/ParentCategory/ParentCategorySendDto';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { CategoryService } from 'src/app/shared/services/category.service';
import { CategoryReceive } from 'src/app/_interfaces/Category/CategoryReceiveDto';
import { CategorySendDto } from 'src/app/_interfaces/Category/CategorySendDto';

@Component({
  selector: 'app-parent-categories',
  templateUrl: './parent-categories.component.html',
  styleUrls: ['./parent-categories.component.css']
})
export class ParentCategoriesComponent implements OnInit {

  parentCategoryForm: FormGroup;
  parentCategories : ParentCategoryReceive[];
  parentCategory : ParentCategorySendDto = null;
  parentCategoryReceive : ParentCategoryReceive = {
    id : null,
    name : null,
    userId : null,
    subcategories : null
  };
  parentCategotyName: string = "";
  isAdding = true;
  isEditing = false;
  showSuccess = false;
  showError = false;
  errorMessage : string;
  successMessage : string;
  modalRef?: BsModalRef;
  message?: string;
  //---------------------------------------------
  categoryForm : FormGroup;
  categories: CategoryReceive[];
  category: CategorySendDto = null;
  categoryReceive : CategoryReceive;
  parentCategoryReceiveForName : ParentCategoryReceive = {
    id : null,
    name : null,
    userId : null,
    subcategories : null
  };
  subcategoriesForCategory : CategoryReceive[];
  categoryName: string = "";
  isAddingModal = true;
  isEditingModal = false;
  showSuccessModal = false;
  showErrorModal = false;
  errorMessageModal : string;
  successMessageModal : string;
  modalRefOnAddingSubcategories : BsModalRef;

  constructor(private parentCategoryAddEditService : ParentCategoryAddEditService, private modalService: BsModalService,
    private categoryService : CategoryService) { }

  openModalOnDelete(template: TemplateRef<any>, id : number) {
    this.modalRef = this.modalService.show(template, {class: 'modal-lg'});
    this.getParentCategory(null, id);
  }

  openModalOnDeleteSubcat(template : TemplateRef<any>, id : number){
    this.modalRefOnAddingSubcategories = this.modalService.show(template, {class: 'modal-lg'});
    this.getCategory(null, id);
  }

  ngOnInit(): void {
    this.loadParentCategories();
    this.loadSubcategories();
    this.parentCategoryForm = new FormGroup({
      categoryName : new FormControl("", [Validators.required])
    })

    this.categoryForm = new FormGroup({
      subcategoryName : new FormControl("", [Validators.required])
    })
  }

  validateControl = (controlName: string) => {
    return this.parentCategoryForm.get(controlName).invalid && this.parentCategoryForm!.get(controlName).touched
  }

  hasError = (controlName: string, errorName: string) => {
    return this.parentCategoryForm.get(controlName).hasError(errorName)
  }

  validateControlModal = (controlName: string) => {
    return this.categoryForm.get(controlName).invalid && this.categoryForm.get(controlName).touched
  }

  hasErrorModal = (controlName: string, errorName: string) => {
    return this.categoryForm.get(controlName).hasError(errorName)
  }

  loadParentCategories(){
    this.parentCategoryAddEditService.getParentCategories("api/parent-categories")
        .subscribe((response: any) => {
          this.parentCategories = response.data;
          console.log(this.parentCategories);
          for(let parent of this.parentCategories){
            this.loadSubcategoriesByParentCategory(parent);
          }
        })
  }

  loadSubcategories(){
    this.categoryService.getCategories("api/categories/list-by-user")
    .subscribe((response : any) => {
      this.categories = response.data;
      for(var category of this.categories){
        if (category.parentTransactionCategoryId !== null){
        this.getParentCategoryForName(category.parentTransactionCategoryId, category);
        }
      }
    })
  }

  loadSubcategoriesByParentCategory(parent: ParentCategoryReceive){
    parent.subcategories = [];
    this.categoryService.getCategories("api/categories/list-by-parent-category/" + parent.id)
      .subscribe((response : any) => {
        this.subcategoriesForCategory = response.data;
        for (let nm of this.subcategoriesForCategory){
          parent.subcategories.push(nm.name);
        }
      })
  }

  getParentCategoryForName(id : number, subcat : CategoryReceive){
    return this.parentCategoryAddEditService.getConcreteParentCategory("api/parent-categories/" + id)
    .subscribe((response : any) => {
      this.parentCategoryReceiveForName = response.data;
      subcat.parentTransactionCategoryName = response.data.name;
    })
  }

  getParentCategory(parentCategoryForm : any, id : number){
    this.parentCategoryAddEditService.getConcreteParentCategory("api/parent-categories/" + id)
    .subscribe((response : any) => {
      console.log(response);
      this.parentCategoryReceive = response.data;
      if (parentCategoryForm !== null){
      const catForm = {... parentCategoryForm}
      this.parentCategotyName = this.parentCategoryReceive.name;
      this.isEditing = true;
      this.isAdding = false;
      this.showSuccess = true;
      this.showError = false;
      this.successMessage = "Category nmb. " + this.parentCategoryReceive.id + " " 
      + this.parentCategoryReceive.name + " has been selected to edit"
      }
    },error => {
      this.showError = true;
      this.showSuccess = false;
      this.errorMessage = error.errorMessage;
    })
  }

  getCategory(categoryForm : any, id : number){
    this.categoryService.getConcreteCategory("api/categories/" + id)
    .subscribe((response : any) => {
      this.categoryReceive = response.data;
      if (categoryForm !== null){
        const subCatForm = {... categoryForm}
        this.categoryName = this.categoryReceive.name;
        this.isEditingModal = true;
        this.isAddingModal = false;
        this.showSuccessModal = true;
        this.showErrorModal = false;
        this.successMessageModal = "Subcategory nmb. " + this.categoryReceive.id + " " 
        + this.categoryReceive.name + " has been selected to edit"
      }
    }, error => {
      this.showErrorModal = true;
      this.showSuccessModal = false;
      this.errorMessageModal = error.errorMessage;
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
      this.loadSubcategories();
      this.loadParentCategories();
    }, error => {
      this.showError = true;
      this.showSuccess = false;
      this.errorMessage = error.errorMessage;
    })
  }

  addCategory(categoryForm : any){
    const subCatForm = {... categoryForm}
    const subCategory : CategorySendDto = {
      name : subCatForm.subcategoryName,
      parentTransactionCategoryId : this.parentCategoryReceive.id
    }
    this.categoryService.addCategory("api/categories", subCategory)
    .subscribe((response : any) => {
      this.showSuccessModal = true;
      this.showErrorModal = false;
      this.successMessageModal = "Subategory " + subCategory.name + " has been added successfuly!"
      this.loadSubcategories();
      this.loadParentCategories();
    }, error => {
      this.showErrorModal = true;
      this.showSuccessModal = false;
      this.errorMessageModal = error.errorMessage;
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
      this.loadSubcategories();
      this.loadParentCategories();
    }, error => {
      this.showError = true;
      this.showSuccess = false;
      this.errorMessage = error.errorMessage;
    })
  }

  editCategory(categoryForm : any, id: number, nameC : string = null){
    var subCategory : CategorySendDto;
    if (categoryForm == null){
      subCategory = {
        parentTransactionCategoryId : this.parentCategoryReceive.id,
      }
    }
    else{
      this.isEditingModal = false;
      this.isAddingModal = true;
      const catForm = {... categoryForm}
      subCategory = {
        name : catForm.subcategoryName
      }
      nameC = subCategory.name;
    }
    this.categoryService.editCategory("api/categories/" + id, subCategory)
    .subscribe((resp : any) => {
      this.showSuccessModal = true;
      this.showErrorModal = false;
      this.successMessageModal = "Subategory " + nameC + " has been edited successfuly!"
      this.loadSubcategories();
      this.loadParentCategories();
    }, error => {
      this.showErrorModal = true;
      this.showSuccessModal = false;
      this.errorMessageModal = error.errorMessage;
    })
  }

  deleteParentCategory(id : number){
    
    this.parentCategoryAddEditService.deleteParentCategory("api/parent-categories/" + id)
        .subscribe((response : any) => {
          this.showSuccess = true;
          this.showError = false;
          this.successMessage = "Category nmb. " + id + " has been deleted successfuly!"
          this.loadSubcategories();
          this.loadParentCategories();
        }, error => {
          this.showError = true;
        this.showSuccess = false;
        this.errorMessage = error.errorMessage;
        })
        this.modalRef?.hide();
  }

  deleteCategory(id : number){
    
    this.categoryService.deleteCategory("api/categories/" + id)
        .subscribe((response : any) => {
          this.showSuccessModal = true;
          this.showErrorModal = false;
          this.successMessageModal = "Subcategory nmb. " + id + " has been deleted successfuly!"
          this.loadSubcategories();
        }, error => {
        this.showErrorModal = true;
        this.showSuccessModal = false;
        this.errorMessageModal = error.errorMessage;
        })
        this.loadSubcategories();
        this.loadParentCategories();
        this.modalRefOnAddingSubcategories?.hide();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  declineModalRef() : void {
    this.modalRefOnAddingSubcategories?.hide();
  }

}
