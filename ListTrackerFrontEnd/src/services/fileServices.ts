import axios from "axios";
import { apiUrl } from "../appConfig";

export async function uploadFile(fileUploadViewModel: any): Promise<any> {
  try {
    const config = {
      method: "post",
      maxBodyLength: Infinity,
      url: `${apiUrl}/uploadFile`,
      data: fileUploadViewModel,
    };

    const response = await axios.request(config);
    return response.data; // Assuming the response contains data you're interested in
  } catch (error) {
    console.log(error);
    throw error; // Re-throw the error to handle it elsewhere if needed
  }
}

export async function getFiles( id:string): Promise<any> {
  try {
    const config = {
      method: "post",
      maxBodyLength: Infinity,
      url: `${apiUrl}/getFiles/?catId=`+id,
      data: {},
    };

    const response = await axios.request(config);
    return response.data; // Assuming the response contains data you're interested in
  } catch (error) {
    console.log(error);
    throw error; // Re-throw the error to handle it elsewhere if needed
  }
}



export async function deleteFile( id:string): Promise<any> {
  try {
    const config = {
      method: "delete",
      maxBodyLength: Infinity,
      url: `${apiUrl}/deleteFile/?id=`+id,
      data: {},
    };

    const response = await axios.request(config);
    return response.data; // Assuming the response contains data you're interested in
  } catch (error) {
    console.log(error);
    throw error; // Re-throw the error to handle it elsewhere if needed
  }
}