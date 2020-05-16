#Things i learned
1/11/20
issue: navigate undefined  - issue: "this" was undefinted in method (or saw "this" as the method itself)... 
solve in one of two ways:
cloneIconClick = (e)=>{
    var docURL =  e.row.data.url; 
    e.event.preventDefault();
    this.router.navigate(["/domains"]);
  }
  
  OR
  this.cloneIconClick = this.cloneIconClick.bind(this);

  OR
  set a global variable
  declare let window: any;
let checkDuplicateText = window.checkDuplicateText;
let selectedDocument = window.selectedDocument;
  -------------------

  1/15/20
  using a class like  public class DocumentParams to pass parameters
  The class contains expected parameter names - 
  These are set on the Angular side like:
  let params: HttpParams = new HttpParams();
        params = params.set("pageNumber", (options.skip / 10 + 1).toString());
        params = params.set("ActId", "84990");

