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
  

  