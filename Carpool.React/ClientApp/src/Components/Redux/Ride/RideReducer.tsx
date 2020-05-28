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
  GET_RIDE_REQUESTS,
  GET_RIDE_REQUESTS_SUCCESS,
  GET_RIDE_REQUESTS_FAILURE,
} from "./RideTypes";

import { RideAction } from "./RideTypes";

import {
  IBookRideResponse,
  IMyBooking,
  IMyOffer,
  IMyOffers,
  IMyBookings,
  IAvailableRide,
  IOfferRide,
  IRideRequest,
  IBookRide,
  IRideRequests,
} from "../../Interfaces";

interface IBool {
  isLoaded: boolean;
  isRequested: boolean;
  isRequestsLoaded: boolean;
}

const intialState: IMyOffers & IBookRideResponse & IMyBookings & IBool = {
  isRequested: false,
  isLoaded: false,
  isRequestsLoaded: false,
  offers: [],
  bookings: [],
  availableRides: [],
};

export function rideReducer(state = intialState, action: RideAction) {
  switch (action.type) {
    case OFFER_RIDE_REQUEST:
      return {
        ...state,
      };
    case OFFER_RIDE_SUCCESS:
      return {
        ...state,
      };
    case OFFER_RIDE_FAILURE:
      return {
        ...state,
      };
    case BOOK_RIDE_REQUEST:
      return {
        ...state,
        isLoaded: false,
        isRequested: true,
      };
    case BOOK_RIDE_FAILURE:
      return {
        ...state,
      };
    case BOOK_RIDE_RESPONSE:
      return {
        ...state,
        availableRides: action.payload,
        isLoaded: true,
      };
    case REQUEST_RIDE:
      return {
        ...state,
      };
    case REQUEST_RIDE_SUCCESS:
      return {
        ...state,
      };
    case REQUEST_RIDE_FAILURE:
      return {
        ...state,
      };
    case GET_MY_BOOKINGS:
      return {
        ...state,
      };
    case GET_MY_BOOKINGS_SUCCESS:
      return {
        ...state,
        bookings: action.payload == undefined ? [] : action.payload,
      };
    case GET_MY_BOOKINGS_FAILURE:
      return {
        ...state,
      };
    case GET_MY_OFFERS:
      return {
        ...state,
      };
    case GET_MY_OFFERS_FAILURE:
      return {
        ...state,
      };
    case GET_MY_OFFERS_SUCCESS:
      return {
        ...state,
        offers: action.payload == undefined ? [] : action.payload,
      };
    case GET_RIDE_REQUESTS:
      return {
        ...state,
        isRequestsLoaded: false,
      };
    case GET_RIDE_REQUESTS_FAILURE:
      return {
        ...state,
      };
    case GET_RIDE_REQUESTS_SUCCESS:
      return {
        ...state,
        requests: action.payload == undefined ? [] : action.payload,
        isRequestsLoaded: true,
      };
    default:
      return state;
  }
}
