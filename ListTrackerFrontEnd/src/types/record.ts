export interface RecordAddressType {
  value: string
  status: number
}
export interface RecordType {
  id: string
  first_name: string
  last_name: string
  address: RecordAddressType
  age: string | number
  phone?: string
  birthday?: string
  gender?: string
  class?: string
  source: string
  status?: string
  records?: RecordType[]
}
