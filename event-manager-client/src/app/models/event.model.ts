export interface Event {
  id?: string; //GUID
  name: string;
  location: string;
  country?: string;
  capacity?: number;
}