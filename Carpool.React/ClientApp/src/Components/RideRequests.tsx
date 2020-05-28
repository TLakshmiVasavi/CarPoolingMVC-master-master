import React from "react";
import axios from "axios";
import { IRideRequests, IRideRequest } from "./Interfaces";
import { Row, Col } from "react-grid-system";
import { MdLocationOn } from "react-icons/md";
import { AppState } from "./Redux/rootReducer";
import { connect, DispatchProp } from "react-redux";
import { approveRideRequest } from "./Redux/Ride/RideActions";

class RideRequests extends React.Component<
  DispatchProps & IBool & IRideRequests,
  {}
> {
  constructor(props: DispatchProps & IBool & IRideRequests) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
  }
  handleSubmit(isApprove: boolean, requestId: number) {
    this.props.approveRideRequest(requestId, this.props.rideId, isApprove);
  }
  render() {
    return (
      <React.Fragment>
        {this.props.isLoaded &&
          this.props.requests.map((item: IRideRequest) => (
            <div className="shadowBox">
              <Row>
                <Col md={4}>
                  <small>From</small>
                  <p>{item.from}</p>
                </Col>
                <Col md={4}>
                  <div className="dot" />
                  <div className="dot" />
                  <div className="dot" />
                  <MdLocationOn className="darkviolet" />
                </Col>
                <Col md={4}>
                  <small>To</small>
                  <p>{item.to}</p>
                </Col>
              </Row>
              <Row>
                <Col md={4}>
                  <small>Cost</small>
                  <p>{item.cost}</p>
                </Col>
                <Col md={4}>
                  <small>Seats Requested</small>
                  <p>{item.noOfSeats}</p>
                </Col>
              </Row>
              <Row>
                <Col md={4}>
                  <button
                    type="submit"
                    className="submit bg-darkorange"
                    onClick={() => this.handleSubmit(true, item.id)}
                  >
                    Accept
                  </button>
                </Col>
                <Col md={4}>
                  <button
                    type="submit"
                    className="submit bg-darkorange"
                    onClick={() => this.handleSubmit(true, item.id)}
                  >
                    Reject
                  </button>
                </Col>
              </Row>
            </div>
          ))}
      </React.Fragment>
    );
  }
}

const mapStateToProps = (state: AppState) => ({
  isLoaded: state.ride.isRequestsLoaded,
  requests: state.ride.requests,
});
interface IBool {
  isLoaded: boolean;
  rideId: number;
}

interface DispatchProps {
  approveRideRequest: (
    requestId: number,
    rideId: number,
    isApprove: boolean
  ) => void;
}
const mapDispatchToProps = {
  approveRideRequest,
};

export default connect(mapStateToProps, mapDispatchToProps)(RideRequests);
