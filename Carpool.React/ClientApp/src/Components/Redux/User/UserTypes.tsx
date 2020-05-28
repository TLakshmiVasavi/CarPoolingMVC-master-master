import { IUser, IVehicles } from "../../Interfaces";
export enum UserEvents {
  USER_LOGIN_REQUEST = "USER_LOGIN_REQUEST",
  USER_LOGIN_SUCCESS = "USER_LOGIN_SUCCESS",
  USER_LOGIN_FAILURE = "USER_LOGIN_FAILURE",
  USER_SIGNUP_REQUEST = "USER_SIGNUP_REQUEST",
  USER_SIGNUP_SUCCESS = "USER_SIGNUP_SUCCESS",
  USER_SIGNUP_FAILURE = "USER_SIGNUP_FAILURE",
  UPDATE_USER_REQUEST = "UPDATE_USER_REQUEST",
  UPDATE_USER_SUCCESS = "UPDATE_USER_SUCCESS",
  UPDATE_USER_FAILURE = "UPDATE_USER_FAILURE",
  LOGOUT_USER = "LOGOUT_USER",
  GET_VEHICLES = "GET_VEHICLES",
  GET_VEHICLES_SUCCESS = "GET_VEHICLES_SUCCESS",
  GET_VEHICLES_FAILURE = "GET_VEHICLES_FAILURE",
  ADD_VEHICLE = "ADD_VEHICLE",
  ADD_VEHICLE_SUCCESS = "ADD_VEHICLE_SUCCESS",
  ADD_VEHICLE_FAILURE = "ADD_VEHICLE_FAILURE",
  GET_USER_IMAGE_FAILURE = "GET_USER_IMAGE_FAILURE",
  GET_USER_IMAGE_SUCCESS = "GET_USER_IMAGE_SUCCESS",
  GET_USER_IMAGE = "GET_USER_IMAGE",
  UPDATE_BALANCE_REQUEST = "UPDATE_BALANCE_REQUEST",
  UPDATE_BALANCE_SUCCESS = "UPDATE_BALANCE_SUCCESS",
  UPDATE_BALANCE_FAILURE = "UPDATE_BALANCE_FAILURE",
}
export interface UpdateBalanceRequest {
  type: typeof UserEvents.UPDATE_BALANCE_REQUEST;
}

export interface UpdateBalanceSuccess {
  type: typeof UserEvents.UPDATE_BALANCE_SUCCESS;
}

export interface UpdateBalanceFailure {
  type: typeof UserEvents.UPDATE_BALANCE_FAILURE;
}

export interface UserLoginRequest {
  type: typeof UserEvents.USER_LOGIN_REQUEST;
}

// export class UserLoginRequest {
//   readonly type = UserLoginRequest;
//   constructor() {}
//}

export interface UserLoginSuccess {
  type: typeof UserEvents.USER_LOGIN_SUCCESS;
  payload: IUser;
}

export interface UserLoginFailure {
  type: typeof UserEvents.USER_LOGIN_FAILURE;
  error: string;
}

export interface UserSignupRequest {
  type: typeof UserEvents.USER_SIGNUP_REQUEST;
}

export interface UserSignupSuccess {
  type: typeof UserEvents.USER_SIGNUP_SUCCESS;
  payload: IUser;
}

export interface UserSignupFailure {
  type: typeof UserEvents.USER_SIGNUP_FAILURE;
  error: string;
}

export interface UpdateUserRequest {
  type: typeof UserEvents.UPDATE_USER_REQUEST;
}

export interface UpdateUserSuccess {
  type: typeof UserEvents.UPDATE_USER_SUCCESS;
  payload: IUser;
}

export interface UpdateUserFailure {
  type: typeof UserEvents.UPDATE_USER_FAILURE;
  error: string;
}

export interface LogoutUser {
  type: typeof UserEvents.LOGOUT_USER;
}

export interface GetVehicles {
  type: typeof UserEvents.GET_VEHICLES;
}

export interface GetVehiclesSuccess {
  type: typeof UserEvents.GET_VEHICLES_SUCCESS;
  payload: IVehicles;
}

export interface GetVehiclesFailure {
  type: typeof UserEvents.GET_VEHICLES_FAILURE;
  error: string;
}

export interface AddVehicle {
  type: typeof UserEvents.ADD_VEHICLE;
}

export interface AddVehicleSuccess {
  type: typeof UserEvents.ADD_VEHICLE_SUCCESS;
}

export interface AddVehicleFailure {
  type: typeof UserEvents.ADD_VEHICLE_FAILURE;
  error: string;
}

export interface GetUserImageSuccess {
  type: typeof UserEvents.GET_USER_IMAGE_SUCCESS;
  payload: any;
}

export interface GetUserImageFailure {
  type: typeof UserEvents.GET_USER_IMAGE_FAILURE;
  error: string;
}

export interface GetUserImage {
  type: typeof UserEvents.GET_USER_IMAGE;
}

export type UserAction =
  | UserLoginSuccess
  | UserLoginFailure
  | UserLoginRequest
  | LogoutUser
  | UserSignupRequest
  | UserSignupFailure
  | UserSignupSuccess
  | UpdateUserRequest
  | UpdateUserSuccess
  | UpdateUserFailure
  | GetVehicles
  | GetVehiclesSuccess
  | GetVehiclesFailure
  | AddVehicle
  | AddVehicleSuccess
  | AddVehicleFailure
  | GetUserImage
  | GetUserImageSuccess
  | GetUserImageFailure;
