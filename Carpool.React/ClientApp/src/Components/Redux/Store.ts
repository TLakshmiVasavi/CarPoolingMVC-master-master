import { createStore, applyMiddleware } from "redux";
import { composeWithDevTools } from "redux-devtools-extension";
import logger from "redux-logger";
import thunk from "redux-thunk";
import { AppState } from "./rootReducer";
import rootReducer from "./rootReducer";
import { configureFlashMessages } from "redux-flash-messages";

export default function configureStore() {
  const store = createStore(
    rootReducer,
    composeWithDevTools(applyMiddleware(logger, thunk))
  );

  return store;
}

export const store: any = configureStore();
configureFlashMessages({
  // The dispatch function for the Redux store.
  dispatch: store.dispatch,
});
