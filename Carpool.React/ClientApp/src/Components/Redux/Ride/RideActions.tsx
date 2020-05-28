import {
  OFFER_RIDE_REQUEST,
  OFFER_RIDE_SUCCESS,
  OFFER_RIDE_FAILURE,
  BOOK_RIDE_REQUEST,
  BOOK_RIDE_FAILURE,
  BOOK_RIDE_RESPONSE,
  REQUEST_RIDE,
  REQUEST_RIDE_SUCCESS,
  REQUEST_RIDE_FAILURE,
  GET_MY_BOOKINGS,
  GET_MY_BOOKINGS_SUCCESS,
  GET_MY_BOOKINGS_FAILURE,
  GET_MY_OFFERS,
  GET_MY_OFFERS_FAILURE,
  GET_MY_OFFERS_SUCCESS,
  APPROVE_REQUEST,
  APPROVE_REQUEST_SUCCESS,
  APPROVE_REQUEST_FAILURE,
  GET_RIDE_REQUESTS,
  GET_RIDE_REQUESTS_SUCCESS,
  GET_RIDE_REQUESTS_FAILURE,
} from "./RideTypes";
import {
  OfferRideRequest,
  OfferRideSuccess,
  OfferRideFailure,
  BookRideRequest,
  BookRideFailure,
  BookRideResponse,
  RequestRide,
  RequestRideSuccess,
  RequestRideFailure,
  GetMyBookings,
  GetMyBookingsSuccess,
  GetMyBookingsFailure,
  GetMyOffers,
  GetMyOffersSuccess,
  GetMyOffersFailure,
  ApproveRequest,
  ApproveRequestSuccess,
  ApproveRequestFailure,
  GetRideRequests,
  GetRideRequestsSuccess,
  GetRideRequestsFailure,
} from "./RideTypes";
import { Dispatch } from "redux";
import axios from "axios";
import {
  IOfferRide,
  IRideRequest,
  IBookRideResponse,
  IMyOffer,
  IMyOffers,
  IMyBooking,
  IRideRequests,
  IBookRide,
  IMyBookings,
} from "../../Interfaces";
import { type } from "os";
import { AppState } from "../rootReducer";
import { toast } from "react-toastify";
import * as actions from "./RideTypes";
import { ActionType } from "typesafe-actions";
export type TodosAction = ActionType<typeof actions>;

export function offerRide(offerRide: IOfferRide) {
  return (
    dispatch: Dispatch<OfferRideSuccess | OfferRideFailure>,
    getState: AppState
  ) => {
    dispatch(OfferRideRequestAction());
    console.log(getState());
    axios
      .post(
        "https://localhost:5001/api/RideApi/OfferRide?userId=" +
          getState().user.mail,
        offerRide
      )
      .then(() => dispatch(OfferRideSuccessAction()))
      .catch((error) => dispatch(OfferRideFailureAction(error)));
  };
}

export function bookRide(Request: IBookRide) {
  return (
    dispatch: Dispatch<BookRideResponse | BookRideFailure>,
    getState: any
  ) => {
    dispatch(BookRideRequestAction());
    axios
      .post(
        "https://localhost:5001/api/RideApi/BookRide?userId=" +
          getState().user.mail,
        Request
      )
      .then((response) => {
        dispatch(BookRideResponseAction(response.data));
      })
      .catch((error) => dispatch(BookRideFailureAction(error)));
  };
}

export function requestRide(
  Request: IBookRide,
  noOfSeats: number,
  rideId: number
) {
  return (
    dispatch: Dispatch<RequestRideSuccess | RequestRideFailure>,
    getState: any
  ) => {
    dispatch(RequestRideAction());
    axios
      .post(
        "https://localhost:5001/api/RideApi/RequestRide?userId=" +
          getState().user.mail +
          "&rideId=" +
          rideId +
          "&noOfSeats" +
          noOfSeats,
        Request
      )
      .then(() => dispatch(RequestRideSuccessAction()))
      .catch((error) => dispatch(RequestRideFailureAction(error)));
  };
}

export function getOffers() {
  return (
    dispatch: Dispatch<GetMyOffersSuccess | GetMyOffersFailure>,
    getState: any
  ) => {
    dispatch(GetMyOffersAction());
    axios
      .get(
        "https://localhost:5001/api/RideApi/GetOfferedRides?userId=" +
          getState().user.mail
      )
      .then((response) => dispatch(GetMyOffersSuccessAction(response.data)))
      .catch((error) => dispatch(GetMyOffersFailureAction(error)));
  };
}

export function getBookings() {
  return (
    dispatch: Dispatch<GetMyOffersSuccess | GetMyOffersFailure>,
    getState: any
  ) => {
    dispatch(GetMyBookingsAction());
    axios
      .get(
        "https://localhost:5001/api/RideApi/GetBookings?userId=" +
          getState().user.mail
      )
      .then((response) => dispatch(GetMyBookingsSuccessAction(response.data)))
      .catch((error) => dispatch(GetMyBookingsFailureAction(error)));
  };
}

export function approveRideRequest(
  requestId: number,
  rideId: number,
  isApprove: boolean
) {
  return (
    dispatch: Dispatch<ApproveRequestSuccess | ApproveRequestFailure>,
    getState: any
  ) => {
    dispatch(ApproveRequestAction());
    axios
      .post(
        "https://localhost:5001/api/RideApi/ApproveRequest?rideId=" +
          rideId +
          "&requestId" +
          requestId +
          "&isApprove" +
          isApprove +
          "&providerId" +
          getState().user.mail
      )
      .then(() => dispatch(ApproveRequestSuccessAction()))
      .catch((error) => dispatch(ApproveRequestFailureAction(error)));
  };
}

export function getRequests(rideId: number) {
  return (
    dispatch: Dispatch<ApproveRequestSuccess | ApproveRequestFailure>,
    getState: any
  ) => {
    dispatch(GetRideRequestsAction());
    axios
      .get(
        "https://localhost:5001/api/RideApi/GetRequests?userId=" +
          getState().user.mail +
          "&rideId=" +
          rideId
      )
      .then((response) => dispatch(GetRideRequestsSuccessAction(response.data)))
      .catch((error) => dispatch(GetRideRequestsFailureAction(error)));
  };
}

export function GetRideRequestsAction() {
  return { type: GET_RIDE_REQUESTS };
}

export function GetRideRequestsSuccessAction(response: IRideRequests) {
  return { type: GET_RIDE_REQUESTS_SUCCESS, payload: response };
}

export function GetRideRequestsFailureAction(error: string) {
  return { type: GET_RIDE_REQUESTS_FAILURE };
}

export function ApproveRequestAction() {
  return { type: APPROVE_REQUEST };
}

export function ApproveRequestSuccessAction() {
  return { type: APPROVE_REQUEST_SUCCESS };
}

export function ApproveRequestFailureAction(error: string) {
  return { type: APPROVE_REQUEST_FAILURE };
}

export function OfferRideRequestAction() {
  return { type: OFFER_RIDE_REQUEST };
}

export function OfferRideSuccessAction() {
  return { type: OFFER_RIDE_SUCCESS };
}

export function OfferRideFailureAction(error: string) {
  return { type: OFFER_RIDE_FAILURE };
}

export function BookRideRequestAction() {
  return { type: BOOK_RIDE_REQUEST };
}

export function BookRideResponseAction(response: IBookRideResponse) {
  return { type: BOOK_RIDE_RESPONSE, payload: response };
}

export function BookRideFailureAction(error: string) {
  return { type: BOOK_RIDE_FAILURE };
}

export function RequestRideAction() {
  return { type: REQUEST_RIDE };
}

export function RequestRideSuccessAction() {
  return { type: REQUEST_RIDE_SUCCESS };
}

export function RequestRideFailureAction(error: string) {
  return { type: REQUEST_RIDE_FAILURE };
}

export function GetMyBookingsAction() {
  return { type: GET_MY_BOOKINGS };
}

export function GetMyBookingsSuccessAction(response: IMyBookings) {
  return { type: GET_MY_BOOKINGS_SUCCESS, payload: response };
}

export function GetMyBookingsFailureAction(error: string) {
  return { type: GET_MY_BOOKINGS_FAILURE };
}

export function GetMyOffersAction() {
  return { type: GET_MY_OFFERS };
}

export function GetMyOffersSuccessAction(response: IMyOffers) {
  return { type: GET_MY_OFFERS_SUCCESS, payload: response };
}

export function GetMyOffersFailureAction(error: string) {
  return { type: GET_MY_OFFERS_FAILURE };
}
