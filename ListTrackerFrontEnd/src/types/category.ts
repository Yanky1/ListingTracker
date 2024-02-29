export interface CategoryType {
  id: string
  categoryName: string
}

export interface FileType {
  id: string
  name: string
  format: string
  size: string
  created_at: string | number
  last_modified: string | number
}

export interface FileUploadViewModel {
  id: number; 
  fileName: string;
  filePath: string;
  fileExtension: string;
  fileSize: string;
  fileContentType: string;
  fileDescription: string;
  isDeleted: boolean;
  dateUploaded: Date;
  categoryId: number;
  file: File;
}
