
import axios from "axios"
import { apiUrl } from "../appConfig";

export async function getCategoryList(): Promise<any> {
    let config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/getCategoryList",
      data: {}
    };
  
    return await axios
      .request(config)
      .then((response) => response)
      .catch((error) => {
        console.log(error);
      });
  }
  
  export async function addCategory(categoryType:any): Promise<any> {
    let config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/addCategory",
      data: {id:0,
      categoryName:categoryType.name}
    };
  
    return await axios
      .request(config)
      .then((response) => response)
      .catch((error) => {
        console.log(error);
      });
  }

  export async function updateCategory(categoryType:any): Promise<any> {
    let config = {
      method: "put",
      maxBodyLength: Infinity,
      url: apiUrl + "/updateCategory",
      data: {id:+categoryType.id,
      categoryName:categoryType.name}
    };
  
    return await axios
      .request(config)
      .then((response) => response)
      .catch((error) => {
        console.log(error);
      });
  }

  export async function deleteCategory(id:string): Promise<any> {
    let config = {
      method: "delete",
      maxBodyLength: Infinity,
      url: apiUrl + "/deleteCategory/"+id,
     
    };
  
    return await axios
      .request(config)
      .then((response) => response)
      .catch((error) => {
        console.log(error);
      });
  }