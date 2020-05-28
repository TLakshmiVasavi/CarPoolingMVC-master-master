import {
  IBookRideResponse,
  IMyOffer,
  IMyBooking,
  IRideRequests,
} from "../../Interfaces";

export const OFFER_RIDE_REQUEST = "OFFER_RIDE_REQUEST";
export const OFFER_RIDE_SUCCESS = "OFFER_RIDE_SUCCESS";
export const OFFER_RIDE_FAILURE = "OFFER_RIDE_FAILURE";
export const BOOK_RIDE_RESPONSE = "BOOK_RIDE_RESPONSE";
export const BOOK_RIDE_REQUEST = "BOOK_RIDE_REQUEST";
export const BOOK_RIDE_FAILURE = "BOOK_RIDE_FAILURE";
export const REQUEST_RIDE = "REQUEST_RIDE";
export const REQUEST_RIDE_SUCCESS = "REQUEST_RIDE_SUCCESS";
export const REQUEST_RIDE_FAILURE = "REQUEST_RIDE_FAILURE";
export const GET_MY_BOOKINGS = "GET_MY_BOOKINGS";
export const GET_MY_BOOKINGS_SUCCESS = "GET_MY_BOOKINGS_SUCCESS";
export const GET_MY_BOOKINGS_FAILURE = "GET_MY_BOOKINGS_FAILURE";
export const GET_MY_OFFERS = "GET_MY_OFFERS";
export const GET_MY_OFFERS_SUCCESS = "GET_MY_OFFERS_SUCCESS";
export const GET_MY_OFFERS_FAILURE = "GET_MY_OFFERS_FAILURE";
export const APPROVE_REQUEST = "APPROVE_REQUEST";
export const APPROVE_REQUEST_SUCCESS = "APPROVE_REQUEST_SUCCESS";
export const APPROVE_REQUEST_FAILURE = "APPROVE_REQUEST_FAILURE";
export const GET_RIDE_REQUESTS = "GET_RIDE_REQUESTS";
export const GET_RIDE_REQUESTS_SUCCESS = "GET_RIDE_REQUESTS_SUCCESS";
export const GET_RIDE_REQUESTS_FAILURE = "GET_RIDE_REQUESTS_FAILURE";

// export class GetRideRequests {
//   readonly type = GET_RIDE_REQUESTS;
//   constructor() {}
// }

export interface GetRideRequests {
  type: typeof GET_RIDE_REQUESTS;
}

export interface GetRideRequestsSuccess {
  type: typeof GET_RIDE_REQUESTS_SUCCESS;
  payload: IRideRequests;
}

export interface GetRideRequestsFailure {
  type: typeof GET_RIDE_REQUESTS_FAILURE;
}

export interface ApproveRequest {
  type: typeof APPROVE_REQUEST;
}

export interface ApproveRequestSuccess {
  type: typeof APPROVE_REQUEST_SUCCESS;
}

export interface ApproveRequestFailure {
  type: typeof APPROVE_REQUEST_FAILURE;
}

export interface OfferRideRequest {
  type: typeof OFFER_RIDE_REQUEST;
}

export interface OfferRideSuccess {
  type: typeof OFFER_RIDE_SUCCESS;
}

export interface OfferRideFailure {
  type: typeof OFFER_RIDE_FAILURE;
}

export interface BookRideRequest {
  type: typeof BOOK_RIDE_REQUEST;
}

export interface BookRideResponse {
  type: typeof BOOK_RIDE_RESPONSE;
  payload: IBookRideResponse;
}

export interface BookRideFailure {
  type: typeof BOOK_RIDE_FAILURE;
}

export interface RequestRide {
  type: typeof REQUEST_RIDE;
}

export interface RequestRideSuccess {
  type: typeof REQUEST_RIDE_SUCCESS;
}

export interface RequestRideFailure {
  type: typeof REQUEST_RIDE_FAILURE;
  error: string;
}

export interface GetMyBookings {
  type: typeof GET_MY_BOOKINGS;
}

export interface GetMyBookingsSuccess {
  type: typeof GET_MY_BOOKINGS_SUCCESS;
  payload: IMyBooking[];
}

export interface GetMyBookingsFailure {
  type: typeof GET_MY_BOOKINGS_FAILURE;
  error: string;
}

export interface GetMyOffers {
  type: typeof GET_MY_OFFERS;
}

export interface GetMyOffersSuccess {
  type: typeof GET_MY_OFFERS_SUCCESS;
  payload: IMyOffer[];
}

export interface GetMyOffersFailure {
  type: typeof GET_MY_OFFERS_FAILURE;
  error: string;
}

export type RideAction =
  | OfferRideRequest
  | OfferRideSuccess
  | OfferRideFailure
  | BookRideRequest
  | BookRideFailure
  | BookRideResponse
  | RequestRide
  | RequestRideSuccess
  | RequestRideFailure
  | GetMyBookingsSuccess
  | GetMyBookings
  | GetMyBookingsFailure
  | GetMyOffersSuccess
  | GetMyOffers
  | GetMyOffersFailure
  | GetRideRequests
  | GetRideRequestsSuccess
  | GetRideRequestsFailure;
