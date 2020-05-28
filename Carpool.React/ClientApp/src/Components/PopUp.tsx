import React from "react";
import "../App.css";
interface IProps {
  handleClose: () => void;
  show: boolean;
  children: any;
}
class PopUpModal extends React.Component<IProps, {}> {
  render() {
    return (
      <div
        className={this.props.show ? "modal d-block" : "modal d-none"}
        onBlur={this.props.handleClose}
      >
        <div className="modal-container">
          <a className="close" onClick={this.props.handleClose}>
            &times;
          </a>
          {this.props.children}
        </div>
      </div>
    );
  }
}
export default PopUpModal;
