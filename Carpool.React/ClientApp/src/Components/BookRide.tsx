import { Formik } from "formik";
import * as Yup from "yup";
import React from "react";
import "../StyleSheets/OfferRide.css";
import { Row, Col } from "react-grid-system";
import "../App.css";
import { MdLocationOn } from "react-icons/md";
import TextField from "@material-ui/core/TextField";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { RouteComponentProps, Redirect } from "react-router-dom";
import { connect } from "react-redux";
import { IBookRide, IAuthDetails } from "./Interfaces";
import { bookRide } from "./Redux/Ride/RideActions";
import { AppState } from "./Redux/rootReducer";
import AvailableRides from "./AvailableRides";
import { vehicleType, times } from "./Interfaces";

const validationSchema = Yup.object({
  from: Yup.string().required("Required"),
  to: Yup.string().required("Required"),
  time: Yup.string().required("Required"),
});

class BookRide extends React.Component<IProps, IBookRide> {
  constructor(props: any) {
    super(props);
    this.state = {
      isChecked: false,
      startDate: new Date(),
      from: "",
      to: "",
      time: "5am-9am",
      vehicleType: "Car",
    };
    this.handleChecked = this.handleChecked.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.dateHandler = this.dateHandler.bind(this);
  }
  dateHandler(e: Date) {
    this.setState({ startDate: e });
  }

  handleChecked() {
    this.setState({ isChecked: !this.state.isChecked });
  }

  handleChange(e: any) {
    const { name, value } = e.target;
    this.setState({ [name]: value });
  }

  render() {
    if (!this.props.isLoggedIn) {
      this.props.history.push("/Login");
    }

    return (
      <div className="OfferRide">
        <Formik
          enableReinitialize
          initialValues={this.state}
          validationSchema={validationSchema}
          onSubmit={() => this.props.bookRide(this.state)}
        >
          {({ handleSubmit, errors }) => (
            <form onSubmit={handleSubmit}>
              <Row>
                <Col md={4}>
                  <div className="shadowBox">
                    <Row>
                      <Col md={8}>
                        <h3>Book a Ride</h3>
                        <small>We get you Rides asap!</small>
                      </Col>
                      <Col md={2}>
                        <label className="switch">
                          <input
                            type="checkbox"
                            className="checkbox"
                            onChange={this.handleChecked}
                          />
                          <span className="slider round"></span>
                        </label>
                      </Col>
                    </Row>

                    <div id="first">
                      <Row>
                        <Col md={8}>
                          <TextField
                            label="From"
                            onChange={this.handleChange}
                            margin="normal"
                            name="from"
                            helperText={errors.from}
                          />
                          <TextField
                            label="To"
                            onChange={this.handleChange}
                            margin="normal"
                            name="to"
                            helperText={errors.to}
                          />
                        </Col>
                        <Col md={2}>
                          <div className="dot bg-darkviolet" />
                          <div className="dot" />
                          <div className="dot" />
                          <div className="dot" />
                          <MdLocationOn />
                        </Col>
                      </Row>
                      <Row>
                        <Col md={8}>
                          <TextField
                            margin="normal"
                            className="bg-white"
                            name="vehicleType"
                            select
                            label="Vehicle"
                            onChange={this.handleChange}
                            SelectProps={{
                              native: true,
                            }}
                          >
                            {vehicleType.map((option) => (
                              <option key={option.label} value={option.label}>
                                {option.label}
                              </option>
                            ))}
                          </TextField>
                        </Col>
                      </Row>

                      <Row>
                        <Col md={8}>
                          <small>Date</small>
                          <DatePicker
                            dateFormat="MM/dd/yyyy"
                            // margin="normal"
                            id="date-picker-inline"
                            //label="Date picker inline"
                            selected={this.state.startDate}
                            onChange={this.dateHandler}
                            minDate={new Date()}
                          />
                        </Col>
                        <Col md={12}>
                          <small>Time</small>
                          <div
                            data-toggle="button"
                            className="btn-group"
                            role="group"
                            aria-label="Basic example"
                          >
                            {times.map((item, index) => (
                              <button
                                name="time"
                                type="button"
                                key={index}
                                className={
                                  this.state.time === item
                                    ? "selected"
                                    : "" + "time"
                                }
                                onClick={this.handleChange}
                                value={item}
                              >
                                {item}
                              </button>
                            ))}
                          </div>
                          <input
                            type="submit"
                            className="submit bg-darkorange"
                            value="Submit"
                            data-test="submit"
                          />
                        </Col>
                      </Row>
                    </div>
                  </div>
                </Col>
                <Col md={8}>
                  <Col id="matches" md={10}>
                    {this.props.isLoaded && <AvailableRides {...this.state} />}
                  </Col>
                </Col>
              </Row>
            </form>
          )}
        </Formik>
      </div>
    );
  }
}

interface DispatchProps {
  bookRide: (ride: IBookRide) => void;
}
interface IProps extends IAuthDetails, RouteComponentProps, DispatchProps {
  isRequested: boolean;
  isLoaded: boolean;
}
const mapStateToProps = (state: AppState) => ({
  isLoggedIn: state.user.isLoggedIn,
  isRequested: state.ride.isRequested,
  isLoaded: state.ride.isLoaded,
});

export default connect(mapStateToProps, { bookRide })(BookRide);
