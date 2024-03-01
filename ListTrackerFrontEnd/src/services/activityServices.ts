import axios from "axios"
import { apiUrl } from "../appConfig";

export async function getLogUploaded(): Promise<any> {
    let config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/getLogUploaded",
    };
  
    return await axios
      .request(config)
      .then((response) => response)
      .catch((error) => {
        console.log(error);
      });
  }

  export async function getLogConflict(id:any): Promise<any> {
    let config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/getLogConflict/?id="+id,
    };
  
    return await axios
      .request(config)
      .then((response) => response)
      .catch((error) => {
        console.log(error);
      });
  }