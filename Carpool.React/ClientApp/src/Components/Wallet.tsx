import React, { Component } from "react";
import { TextField } from "@material-ui/core";
import "../App.css";
import "../StyleSheets/Colors.css";
import { RouteComponentProps } from "react-router-dom";
import { Formik } from "formik";
import * as Yup from "yup";
import { updateBalance } from "./Redux/User/UserActions";
import { connect } from "react-redux";
import { IAuthDetails } from "./Interfaces";
import { AppState } from "./Redux/rootReducer";

const validationSchema = Yup.object({
  amount: Yup.number().required("Required"),
});

interface IState {
  amount: number;
}

class Wallet extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      amount: 0,
    };
  }

  render() {
    return (
      <Formik
        enableReinitialize
        initialValues={this.state}
        validationSchema={validationSchema}
        onSubmit={(values) => {
          this.props.updateBalance(values.amount);
        }}
      >
        {({ handleSubmit, handleChange, errors }) => (
          <form onSubmit={handleSubmit}>
            <TextField
              margin="normal"
              className="bg-white rounded-corners"
              type="number"
              onChange={handleChange}
              name={errors.amount ?? "amount"}
              label="Amount"
            />
            <button type="submit" className="submit bg-darkorange">
              Submit
            </button>
          </form>
        )}
      </Formik>
    );
  }
}

interface IProps extends DispatchProps, IAuthDetails {}

const mapStateToProps = (state: AppState) => ({
  isLoggedIn: state.user.isLoggedIN,
});

interface DispatchProps {
  updateBalance: (amount: number) => void;
}

export default connect(null, { updateBalance })(Wallet);
