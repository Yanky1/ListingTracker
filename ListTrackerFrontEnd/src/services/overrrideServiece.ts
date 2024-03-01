import axios from "axios"
import { apiUrl } from "../appConfig";

export async function getAccecptedPerson(id:any): Promise<any> {
    let config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/getAcceptedPerson/?id="+id,
    };
  
    return await axios
      .request(config)
      .then((response) => response)
      .catch((error) => {
        console.log(error);
      });
  }

  export async function updateAccecptedPerson(ds:any): Promise<any> {
    let config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/updateAcceptedPerson",
      headers: {
        "Content-Type": "application/json"
      },
      data: { updateData: JSON.stringify(ds) }
    };
  
    return await axios
      .request(config)
      .then((response) => response)
      .catch((error) => {
        console.log(error);
      });
  }