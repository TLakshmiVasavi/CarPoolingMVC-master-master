import React from "react";
import { UserEvents } from "./UserTypes";
import {
  UserLoginSuccess,
  UserLoginFailure,
  UserSignupFailure,
  UserSignupSuccess,
  UpdateUserSuccess,
  UpdateUserFailure,
  AddVehicleSuccess,
  AddVehicleFailure,
  GetVehiclesFailure,
  GetVehiclesSuccess,
  LogoutUser,
  UpdateBalanceFailure,
  UpdateBalanceSuccess,
} from "./UserTypes";
import axios from "axios";
import {
  IUser,
  INewUser,
  IAuthUser,
  IVehicles,
  IVehicle,
  IImage,
} from "../../Interfaces";
import { Dispatch } from "redux";
import { AppState } from "../rootReducer";
import "react-toastify/dist/ReactToastify.css";
import { toast } from "react-toastify";
import { createAction } from "typesafe-actions";

export function Login(user: IAuthUser) {
  return (dispatch: Dispatch<UserLoginSuccess | UserLoginFailure>) => {
    dispatch(UserLoginRequestAction());
    axios
      .post("https://localhost:5001/api/UserApi/Login", user)
      .then((response) => {
        if (response.data.isSuccess) {
          dispatch(UserLoginSuccessAction(response.data.user));
          dispatch(GetUserImageAction());
          axios
            .get(
              "https://localhost:5001/api/UserApi/GetImage?userId=" +
                response.data.user.mail
            )
            .then((response) =>
              dispatch(GetUserImageSuccessAction(response.data))
            )
            .catch((error) => dispatch(GetUserImageFailureAction(error)));
        } else {
          toast.error(response.data.errorMessage);
          dispatch(UserLoginFailureAction(response.data.errorMessage));
        }
      })
      .catch((error) => {
        toast.error("Server Not Responding");
        dispatch(UserLoginFailureAction(error));
      });
  };
}

export function Logout() {
  return (dispatch: Dispatch<LogoutUser>, getState: AppState) => {
    axios
      .post(
        "https://localhost:5001/api/UserApi/Logout?userId=" +
          getState().user.mail
      )
      .then(() => dispatch(LogoutUserAction()));
  };
}

export function Signup(user: INewUser) {
  return (dispatch: Dispatch<UserSignupSuccess | UserSignupFailure>) => {
    dispatch(UserSignupRequestAction(user));
    const data = new FormData();
    Object.keys(user).map((i) => data.append(i, user[i]));
    axios
      .post("https://localhost:5001/api/UserApi/SignUp", data)
      .then((response) => dispatch(UserSignupSuccessAction(response.data.user)))
      .catch((error) => dispatch(UserSignupFailureAction(error)));
  };
}

export function UpdateUser(user: IUser) {
  return (
    dispatch: Dispatch<UpdateUserSuccess | UpdateUserFailure>,
    getState: AppState
  ) => {
    dispatch(UpdateUserRequestAction());
    const data = new FormData();
    Object.keys(user).map((i) => data.append(i, user[i]));
    axios
      .post(
        "https://localhost:5001/api/UserApi/Update?userId=" +
          getState().user.mail,
        data
      )
      .then((response) => dispatch(UpdateUserSuccessAction(response.data.user)))
      .catch((error) => dispatch(UpdateUserFailureAction(error)));
  };
}

export function getVehicles() {
  return (
    dispatch: Dispatch<GetVehiclesSuccess | GetVehiclesFailure>,
    getState: AppState
  ) => {
    dispatch(GetVehiclesAction());
    axios
      .get(
        "https://localhost:5001/api/UserApi/GetVehicles?userId=" +
          getState().user.mail
      )
      .then((response) => {
        console.log(response.data);
        dispatch(GetVehiclesSuccessAction(response.data));
      })
      .catch((error) => dispatch(GetVehiclesFailureAction(error)));
  };
}

export function addVehicle(vehicle: IVehicle) {
  return (
    dispatch: Dispatch<AddVehicleSuccess | AddVehicleFailure>,
    getState: AppState
  ) => {
    dispatch(AddVehicleAction());
    axios
      .post(
        "https://localhost:5001/api/UserApi/AddVehicle?userId=" +
          getState().user.mail,
        vehicle
      )
      .then(() => dispatch(AddVehicleSuccessAction()))
      .catch((error) => dispatch(AddVehicleFailureAction(error)));
  };
}

export function updateBalance(amount: number) {
  return (
    dispatch: Dispatch<UpdateBalanceSuccess | UpdateBalanceFailure>,
    getState: AppState
  ) => {
    dispatch(UpdateBalanceAction());
    axios
      .post(
        "https://localhost:5001/api/UserApi/UpdateBalance?userId=" +
          getState().user.mail,
        amount
      )
      .then(() => dispatch(UpdateBalanceSuccessAction()))
      .catch((error) => dispatch(UpdateBalanceFailureAction(error)));
  };
}
//export interface UpdateBalanceAction  =ReturnType<UpdateBalanceAction>
export const UpdateBalanceAction = createAction(
  UserEvents.UPDATE_BALANCE_REQUEST
)<void>();
// export type  =ReturnType<>
export const UserLoginRequestAction = createAction(
  UserEvents.USER_LOGIN_REQUEST
)<void>();
// export type  =ReturnType<>
export const UserLoginFailureAction = createAction(
  UserEvents.USER_LOGIN_FAILURE
)<string>();
// export type  =ReturnType<>
export const UpdateBalanceSuccessAction = createAction(
  UserEvents.UPDATE_BALANCE_SUCCESS
)<void>();
// export type  =ReturnType<>
export const UpdateBalanceFailureAction = createAction(
  UserEvents.UPDATE_BALANCE_FAILURE
)<string>();
// export type  =ReturnType<>
export const GetVehiclesAction = createAction(UserEvents.GET_VEHICLES)<void>();
// export type  =ReturnType<>
export const GetVehiclesSuccessAction = createAction(
  UserEvents.GET_VEHICLES_SUCCESS
)<IVehicles>();
// export type  =ReturnType<>
export const GetVehiclesFailureAction = createAction(
  UserEvents.GET_VEHICLES_FAILURE
)<string>();
// export type  =ReturnType<>
export const UserLoginSuccessAction = createAction(
  UserEvents.USER_LOGIN_SUCCESS
)<IUser>();
// export type  =ReturnType<>
export const GetUserImageAction = createAction(UserEvents.GET_USER_IMAGE)<
  void
>();
// export type  =ReturnType<>
export const GetUserImageSuccessAction = createAction(
  UserEvents.GET_USER_IMAGE_SUCCESS
)<IImage>();
// export type  =ReturnType<>
export const GetUserImageFailureAction = createAction(
  UserEvents.GET_USER_IMAGE_FAILURE
)<string>();
// export type  =ReturnType<>
export const LogoutUserAction = createAction(UserEvents.LOGOUT_USER)<void>();
// export type  =ReturnType<>
export const UserSignupRequestAction = createAction(
  UserEvents.USER_SIGNUP_REQUEST
)<INewUser>();
// export type  =ReturnType<>
export const UserSignupSuccessAction = createAction(
  UserEvents.USER_SIGNUP_SUCCESS
)<IUser>();
// export type  =ReturnType<>
export const UserSignupFailureAction = createAction(
  UserEvents.USER_SIGNUP_FAILURE
)<string>();
// export type  =ReturnType<>
export const UpdateUserRequestAction = createAction(
  UserEvents.UPDATE_USER_REQUEST
)<void>();
// export type  =ReturnType<>
export const UpdateUserSuccessAction = createAction(
  UserEvents.UPDATE_USER_SUCCESS
)<IUser>();
// export type  =ReturnType<>
export const UpdateUserFailureAction = createAction(
  UserEvents.UPDATE_USER_FAILURE
)<string>();
// export type  =ReturnType<>
export const AddVehicleAction = createAction(UserEvents.ADD_VEHICLE)<void>();
// export type  =ReturnType<>
export const AddVehicleSuccessAction = createAction(
  UserEvents.ADD_VEHICLE_SUCCESS
)<void>();
// export type  =ReturnType<>
export const AddVehicleFailureAction = createAction(
  UserEvents.ADD_VEHICLE_FAILURE
)<string>();
interface IStringMap<T> {
  [key: string]: T;
}
type IAnyFunction = (...args: any[]) => any;
type IActionUnion<A extends IStringMap<IAnyFunction>> = ReturnType<A[keyof A]>;
const actionss = {
  UpdateBalanceAction,
  UserLoginRequestAction,
  UserLoginFailureAction,
  UpdateBalanceSuccessAction,
  UpdateBalanceFailureAction,
  GetVehiclesAction,
  GetVehiclesSuccessAction,
  GetVehiclesFailureAction,
  UserLoginSuccessAction,
  GetUserImageAction,
  GetUserImageSuccessAction,
  GetUserImageFailureAction,
  LogoutUserAction,
  UserSignupRequestAction,
  UserSignupSuccessAction,
  UserSignupFailureAction,
  UpdateUserRequestAction,
  UpdateUserSuccessAction,
  UpdateUserFailureAction,
  AddVehicleAction,
  AddVehicleSuccessAction,
  AddVehicleFailureAction,
};

export type IActionss = IActionUnion<typeof actionss>;
