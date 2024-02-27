export interface CategoryType {
  id: string
  name: string
}

export interface FileType {
  id: string
  name: string
  format: string
  size: string
  created_at: string | number
  last_modified: string | number
}