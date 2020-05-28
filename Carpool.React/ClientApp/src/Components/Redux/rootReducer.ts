import { combineReducers } from "redux";
// import BookingReducer from './Booking/BookingReducer'
import { rideReducer } from "./Ride/RideReducer";
import { userReducer } from "./User/UserReducer";
import { store } from "./Store";
import { flashMessage } from "redux-flash-messages";

const rootReducer = combineReducers({
  user: userReducer,
  ride: rideReducer,
  message: flashMessage,
});

export default rootReducer;

export type AppState = ReturnType<typeof store.getState>;
