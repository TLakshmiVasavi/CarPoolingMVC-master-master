import { UserEvents } from "./UserTypes";
// import {
//   USER_LOGIN_REQUEST,
//   USER_LOGIN_SUCCESS,
//   USER_LOGIN_FAILURE,
//   UPDATE_USER_FAILURE,
//   UPDATE_USER_REQUEST,
//   UPDATE_USER_SUCCESS,
//   LOGOUT_USER,
//   USER_SIGNUP_REQUEST,
//   USER_SIGNUP_SUCCESS,
//   USER_SIGNUP_FAILURE,
//   GET_VEHICLES,
//   GET_VEHICLES_SUCCESS,
//   GET_VEHICLES_FAILURE,
//   ADD_VEHICLE,
//   ADD_VEHICLE_SUCCESS,
//   ADD_VEHICLE_FAILURE,
//   GET_USER_IMAGE_SUCCESS,
//   GET_USER_IMAGE,
//   GET_USER_IMAGE_FAILURE,
// } from "./UserTypes";
import { UserAction } from "./UserTypes";
import { INewUser, IUser, IAuthUser, IVehicles } from "../../Interfaces";
import * as actions from "./UserActions";
import { IActionss } from "./UserActions";
import { ActionType } from "typesafe-actions";
export type TestAction = ActionType<typeof actions>;
interface IBool {
  isLoading: boolean;
}
const user: IUser & IVehicles & IBool = {
  isLoading: true,
  name: "",
  mail: "",
  age: 20,
  number: "",
  photo: null,
  gender: "Female",
  isLoggedIn: false,
  error: "",
  disable: false,
  vehicles: [],
};

export function userReducer(
  state = user,
  action: IActionss
): IUser & IVehicles & IBool {
  switch (action.type) {
    case UserEvents.USER_LOGIN_SUCCESS:
      return {
        ...state,
        ...action.payload,
        isLoggedIn: true,
      };
    case UserEvents.USER_LOGIN_FAILURE:
      return {
        ...state,
        error: action.payload,
      };
    case UserEvents.LOGOUT_USER:
      return {
        ...state,
        user: null,
        isLoggedIn: false,
      };
    case UserEvents.UPDATE_USER_SUCCESS:
      return {
        ...state,
        ...action.payload,
      };
    case UserEvents.UPDATE_USER_FAILURE:
      return {
        ...state,
        error: action.payload,
      };
    case UserEvents.GET_VEHICLES_SUCCESS:
      return {
        ...state,
        vehicles: action.payload.vehicles,
        isLoading: false,
      };
    case UserEvents.GET_VEHICLES_FAILURE:
      return {
        ...state,
        error: action.payload,
      };
    case UserEvents.ADD_VEHICLE_FAILURE:
      return {
        ...state,
        error: action.payload,
      };
    case UserEvents.GET_VEHICLES:
      return {
        ...state,
        isLoading: true,
      };
    case UserEvents.GET_USER_IMAGE_SUCCESS:
      return {
        ...state,
        photo: action.payload,
      };
    case UserEvents.GET_USER_IMAGE_FAILURE:
      return {
        ...state,
        error: action.payload,
      };
    case UserEvents.GET_USER_IMAGE:

    case UserEvents.USER_LOGIN_REQUEST:
    case UserEvents.USER_SIGNUP_REQUEST:
    case UserEvents.USER_SIGNUP_SUCCESS:
    case UserEvents.USER_SIGNUP_FAILURE:
    case UserEvents.UPDATE_USER_REQUEST:
    case UserEvents.ADD_VEHICLE:
    case UserEvents.ADD_VEHICLE_SUCCESS:
    default:
      return state;
  }
}
