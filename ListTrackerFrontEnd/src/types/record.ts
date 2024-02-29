export interface RecordAddressType {
  value: string
  status: number
}
// export interface RecordType {
//   id: string
//   first_name: string
//   last_name: string
//   address: string
//   age: string | number
//   phone?: string
//   birthday?: string
//   gender?: string
//   class?: string
//   source: string
//   status?: string
//   records?: RecordType[]
// }
export interface RecordType {
  id: string;
  first_name: string;
  last_name: string;
  email: string;
  phoneNumber: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  status: string;
  records?: RecordType[];
}
