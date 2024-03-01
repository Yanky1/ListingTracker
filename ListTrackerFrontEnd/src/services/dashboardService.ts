import axios from "axios"
import { apiUrl } from "../appConfig";

export async function getPersonList(): Promise<any> {
    let config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/getAcceptedPersonWithMatchingRecords",
      data: {}
    };
  
    return await axios
      .request(config)
      .then((response) => response)
      .catch((error) => {
        console.log(error);
      });
  }
  
export async function updateConflict(updateData:any): Promise<any> {
  let config = {
    method: "post",
    url: apiUrl + "/updateConflict",
    headers: {
      "Content-Type": "application/json"
    },
    data: { updateData: JSON.stringify(updateData) }
  };

  return await axios
    .request(config)
    .then((response) => response)
    .catch((error) => {
      console.log(error);
    });
}


export async function deleteperson(id:string): Promise<any> {
  let config = {
    method: "delete",
    maxBodyLength: Infinity,
    url: apiUrl + "/deletePerson/?id="+id,
   
  };

  return await axios
    .request(config)
    .then((response) => response)
    .catch((error) => error);
}

export async function getPersonById(id:string): Promise<any> {
  let config = {
    method: "post",
    maxBodyLength: Infinity,
    url: apiUrl + "/getAcceptedPersonWithMatchingRecordById/?id="+id,
   
  };

  return await axios
    .request(config)
    .then((response) => response)
    .catch((error) => error);
}