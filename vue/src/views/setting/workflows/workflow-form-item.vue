<template>
    <div>
        <FormItem
            :label="element.type != 'paragraph' ? element.label : ' '"
            :prop="element.name"
        >
            <Input
                placeholder="Enter something..."
                v-if="element.type == 'text'"
                v-model="value"
                :style="styles"
                :disabled="disabled"
            ></Input>
            <Input
                type="textarea"
                placeholder="Enter something..."
                v-if="element.type == 'textarea'"
                :disabled="disabled"
                v-model="value"
                :style="styles"
            ></Input>
            <CheckboxGroup
                v-if="element.type == 'checkbox'"
                v-model="value"
                :disabled="disabled"
                :style="styles"
            >
                <Checkbox
                    :label="i.value"
                    v-for="(i, key) in element.items"
                    :key="key"
                >
                    {{ i.label }}
                </Checkbox>
            </CheckboxGroup>
            <RadioGroup
                v-if="element.type == 'radio'"
                v-model="value"
                :disabled="disabled"
                :style="styles"
            >
                <Radio
                    :label="i.label"
                    :value="i.value"
                    v-for="(i, key) in element.items"
                    :key="key"
                ></Radio>
            </RadioGroup>
            <Select
                v-if="element.type == 'select'"
                v-model="value"
                :disabled="disabled"
                :style="styles"
            >
                <Option
                    :value="i.value"
                    v-for="(i, key) in element.items"
                    :key="key"
                >
                    {{ i.label }}
                </Option>
            </Select>
            <DatePicker
                type="date"
                v-if="element.type == 'datepicker'"
                placeholder="Select date"
                v-model="value"
                :disabled="disabled"
                :style="styles"
            ></DatePicker>

            <DatePicker
                type="daterange"
                confirm
                placement="bottom-end"
                v-if="element.type == 'daterangepicker'"
                placeholder="Select date"
                v-model="value"
                :disabled="disabled"
                :style="styles"
            ></DatePicker>
            <Rate
                v-if="element.type == 'rate'"
                v-model="value"
                :disabled="disabled"
                :style="styles"
            />
            <InputNumber
                v-model="value"
                :disabled="disabled"
                :style="styles"
                v-if="element.type == 'number'"
            ></InputNumber>
            <InputNumber
                v-if="element.type == 'money'"
                v-model="value"
                :formatter="
                    (value) =>
                        `¥ ${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')
                "
                :parser="(value) => value.replace(/\$\s?|(,*)/g, '')"
                :precision="2"
                :style="styles"
                :active-change="false"
                :disabled="disabled"
            ></InputNumber>
            <i-switch
                v-model="value"
                :style="styles"
                :disabled="disabled"
                v-if="element.type == 'switch'"
            />
            <p v-if="element.type == 'paragraph'" :style="styles">
                {{ element.label }}
            </p>
            <upload-images
                v-model="value"
                :style="styles"
                v-if="element.type == 'uploadImages'"
                :disabled="disabled"
            ></upload-images>
        </FormItem>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Watch } from "vue-property-decorator";
import AbpBase from "@/lib/abpbase";
import UploadImages from "@/components/upload-images.vue";

@Component({
    components: {
        UploadImages,
    },
})
export default class WorkflowFormItem extends AbpBase {
    @Prop({ default: null })
    readonly value: any;

    @Prop({ default: null })
    readonly element: any;

    @Prop({ type: Boolean, default: false })
    readonly disabled: any;

    get styles() {
        let style = {};
        if (this.element.styles) {
            this.element.styles.forEach((e) => {
                style[e.name] = e.value;
            });
        }
        return style;
    }

    @Watch("value")
    update() {
        this.$emit("input", this.value);
    }
}
</script>

