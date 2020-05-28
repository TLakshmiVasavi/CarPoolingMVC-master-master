import React from "react";
import { Container, Row, Col } from "react-grid-system";
import { IMyBooking, IMyBookings } from "./Interfaces";
import { MdLocationOn } from "react-icons/md";
import { AppState } from "./Redux/rootReducer";
import { getBookings } from "./Redux/Ride/RideActions";
import { connect } from "react-redux";

class BookedRides extends React.Component<IMyBookings & DispatchProps, {}> {
  componentWillMount() {
    this.props.getBookings();
  }
  render() {
    var url = "data:image/png;base64,";
    return (
      <>
        {this.props.bookings.map((ride: IMyBooking) => (
          <div className="shadowBox">
            <Row>
              <Col md={8}>
                <h2>{ride.providerName}</h2>
              </Col>
              <Col md={4}>
                <img src={url + ride.providerPic} className="imgRound" />
              </Col>
            </Row>
            <Row>
              <Col md={4}>
                <small>From</small>
                <p>{ride.from}</p>
              </Col>
              <Col md={4}>
                <div className="dot" />
                <div className="dot" />
                <div className="dot" />
                <MdLocationOn className="darkviolet" />
              </Col>
              <Col md={4}>
                <small>To</small>
                <p>{ride.to}</p>
              </Col>
            </Row>
            <Row>
              <Col md={4}>
                <small>Date</small>
                <p>{ride.startDate}</p>
              </Col>
              <Col md={4} />
              <Col md={4}>
                <small>Time</small>
                <p>{ride.time}</p>
              </Col>
            </Row>
            <Row>
              <Col md={4}>
                <small>Price</small>
                <p>{ride.cost}</p>
              </Col>
              <Col md={4} />
              <Col md={4}>
                <small>Seats Available</small>
                <p>{ride.noOfSeats}</p>
              </Col>
            </Row>
            <Row>
              <Col md={4}>
                <small>Ride Status</small>
                <p>{ride.rideStatus}</p>
              </Col>
              <Col md={4} />
              <Col md={4}>
                <small>Request Status</small>
                <p>{ride.requestStatus}</p>
              </Col>
            </Row>
            <Row>
              <Col md={4}>
                <small>Vehicle</small>
                <p>{ride.vehicleType}</p>
              </Col>
              <Col md={4} />
              <Col md={4}>
                <small>Vehicle Number</small>
                <p>{ride.vehicleNumber}</p>
              </Col>
            </Row>
          </div>
        ))}
      </>
    );
  }
}

interface DispatchProps {
  getBookings: () => void;
}
const mapDispatchToProps = {
  getBookings,
};

const mapStateToProps = (state: AppState) => ({
  bookings: state.ride.bookings,
});

export default connect(mapStateToProps, mapDispatchToProps)(BookedRides);
