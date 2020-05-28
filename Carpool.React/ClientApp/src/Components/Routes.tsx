import React from "react";
import { Switch, Route } from "react-router-dom";
import LoginSignUp from "./LoginSignUp";
import Dashboard from "./DashBoard";
import MyRides from "./MyRides";
import BookRide from "./BookRide";
import UserProfile from "./UserProfile";
import OfferRide from "./OfferRide";
import AddVehicle from "./AddVehicle";
import Wallet from "./Wallet";

export default function Routes() {
  return (
    <Switch>
      <Route path="/(Login|SignUp|)/" component={LoginSignUp} />
      <Route path="/Profile" component={UserProfile} />
      <Route path="/Dashboard" component={Dashboard} />
      <Route path="/MyRides" component={MyRides} />
      <Route path="/OfferRide" component={OfferRide} />
      <Route path="/BookRide" component={BookRide} />
      <Route path="/AddVehicle" component={AddVehicle} />
      <Route path="/Wallet" component={Wallet} />
    </Switch>
  );
}
