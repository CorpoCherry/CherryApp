import mdcAutoInit from '@material/auto-init';
import { MDCTextField } from '@material/textfield';
import { MDCCheckbox } from '@material/checkbox';
mdcAutoInit.register('MDCTextField', MDCTextField);
mdcAutoInit.register('MDCCheckbox', MDCCheckbox);
mdcAutoInit();